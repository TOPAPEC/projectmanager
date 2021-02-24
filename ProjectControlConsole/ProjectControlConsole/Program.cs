using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectControlClasses;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProjectControlConsole
{
    class Program
    {
        /*
        Do not close program with methods other then safexit button, otherwise all data will be lost.
        Only latin characters and digits are available for naming users, projects, tasks.
        Keywords for level of task: epic, story, task, bug.
        Keywords for status of task: opened, workinprogress, completed.
        List of commands:
            help - list of commands.
            safexit - automaticly save data and exit program, otherwise data will be lost.
            createuser username
            userlist
            removeuser username
            createproject projectname maxtasksvalue
            projectlist
            changeprojectname oldname newname
            removeproject projectname
            addtasktoproject projectname taskname tasklevel
            addusertotask projectname taskname username
            removeuserfromtask projectname taskname username
            changetaskstatus projectname taskname newstatus
            tasklist projectname
            groupedtasklist projectname
            removetaskfromproject projectname taskname
            addsubtasktoepictask projectname taskname subtaskname subtasklevel
            removesubtaskfromepictask projectname taskname subtaskname
            changeepictasksubtaskstatus projectname taskname subtaskname newstatus.
            epictasksubtasklist projectname taskname.
            epictaskgroupedsubtasklist projectname taskname.
            addusertoepictasksubtask projectname taskname username.
            removeuserfromepictasksubtask projectname taskname username.

         You should separate all arguments with space. Names should not contain ones.
         */
        static void Main(string[] args)
        {
            ProjectManager projectManager = new ProjectManager();

            try
            {
                if (File.Exists("data.bin"))
                {
                    var fileStream = new FileStream(@"data.bin", FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();
                    projectManager = (ProjectManager)formatter.Deserialize(fileStream);
                    fileStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while loading saved files. {ex.Message}");
            }
            Console.WriteLine("Welcome to Cyberpunk2077 Console Edition. Here are some rules:");
            DisplayHelp();
            try
            {
                do
                {
                    CommandParser(projectManager);
                } while (true);
            }
            catch (Exception) { }

            try
            {
                var fileStream = new FileStream(@"data.bin", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, projectManager);
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save data. {ex.Message}");
            }



        }

        /// <summary>
        /// Gets command from user.
        /// </summary>
        static void CommandParser(ProjectManager projectManager)
        {
            Console.Write("> ");
            string command = Console.ReadLine();
            if (command == "safexit")
                throw new Exception("Save and exit.");
            if (command == "help")
                DisplayHelp();
            string[] splitedCommand = command.Trim().Split(' ');
            try
            {
                CommandCheck(splitedCommand);
                switch (splitedCommand[0])
                {
                    case "createuser":
                        projectManager.AddUser(splitedCommand[1]);
                        break;
                    case "userlist":
                        foreach (var userName in projectManager.GetUserList())
                            Console.WriteLine(userName);
                        break;
                    case "removeuser":
                        projectManager.RemoveUser(splitedCommand[1]);
                        break;
                    case "createproject":
                        projectManager.CreateProject(splitedCommand[1], int.Parse(splitedCommand[2]));
                        break;
                    case "projectlist":
                        foreach (var project in projectManager.GetProjectList())
                            Console.WriteLine(project);
                        break;
                    case "changeprojectname":
                        projectManager.ChangeProjectName(splitedCommand[1], splitedCommand[2]);
                        break;
                    case "removeproject":
                        projectManager.RemoveProject(splitedCommand[1]);
                        break;
                    case "addtasktoproject":
                        projectManager.GetProject(splitedCommand[1]).CreateNewTask(splitedCommand[2], StringToTaskLevel(splitedCommand[3]));
                        break;
                    case "addusertotask":
                        projectManager.AssignUserToTask(splitedCommand[1], splitedCommand[2], splitedCommand[3]);
                        break;
                    case "removeuserfromtask":
                        projectManager.RemoveUserFromTask(splitedCommand[1], splitedCommand[2], splitedCommand[3]);
                        break;
                    case "changetaskstatus":
                        projectManager.GetProject(splitedCommand[1]).ChangeTaskStatus(splitedCommand[2], StringToTaskStatus(splitedCommand[3]));
                        break;
                    case "tasklist":
                        foreach (var taskinfo in projectManager.GetProject(splitedCommand[1]).GetTaskList())
                            Console.WriteLine(taskinfo);
                        break;
                    case "groupedtasklist":
                        foreach (var taskinfo in projectManager.GetProject(splitedCommand[1]).GetOrderedByStatusTaskList())
                            Console.WriteLine(taskinfo);
                        break;
                    case "removetaskfromproject":
                        projectManager.GetProject(splitedCommand[1]).RemoveTask(splitedCommand[2]);
                        break;
                    case "addsubtasktoepictask":
                        projectManager.GetProject(splitedCommand[1]).AddTaskToEpicTask(splitedCommand[2], splitedCommand[3], StringToTaskLevel(splitedCommand[4]));
                        break;
                    case "removesubtaskfromepictask":
                        projectManager.GetProject(splitedCommand[1]).RemoveSubtaskFromEpicTask(splitedCommand[2], splitedCommand[3]);
                        break;
                    case "changeepictasksubtaskstatus":
                        projectManager.GetProject(splitedCommand[1]).GetEpicTask(splitedCommand[2]).ChangeTaskStatus(splitedCommand[3], StringToTaskStatus(splitedCommand[4]));
                        break;
                    case "epictasksubtasklist":
                        foreach (var taskinfo in projectManager.GetProject(splitedCommand[1]).GetEpicTask(splitedCommand[2]).GetSubtaskList())
                            Console.WriteLine(taskinfo);
                        break;
                    case "epictaskgroupedsubtasklist":
                        foreach (var taskinfo in projectManager.GetProject(splitedCommand[1]).GetEpicTask(splitedCommand[2]).GetOrderedByStatusSubtaskList())
                            Console.WriteLine(taskinfo);
                        break;
                    case "addusertoepictasksubtask":
                        projectManager.GetProject(splitedCommand[1]).GetEpicTask(splitedCommand[2]).AssignUserToSubtask(splitedCommand[3], splitedCommand[4]);
                        break;
                    case "removeuserfromepictasksubtask":
                        projectManager.GetProject(splitedCommand[1]).GetEpicTask(splitedCommand[2]).RemoveUserFromSubtask(splitedCommand[3], splitedCommand[4]);
                        break;
                    default:
                        throw new ArgumentException("Invalid command.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine($"Do not close program with methods other then safexit button, otherwise all data will be lost.{Environment.NewLine}" +
                                $"Only latin characters and digits are available for naming users, projects, tasks.{Environment.NewLine}" +
                                $"Keywords for level of task: epic, story, task, bug.{Environment.NewLine}" +
                                $"Keywords for status of task: opened, workinprogress, completed.{Environment.NewLine}" +
                                $"List of commands:{Environment.NewLine}" +
                                $"    help - list of commands.{Environment.NewLine}" +
                                $"    safexit - automaticly save data and exit program, otherwise data will be lost.{Environment.NewLine}" +
                                $"    createuser username{Environment.NewLine}" +
                                $"    userlist{Environment.NewLine}" +
                                $"    removeuser username{Environment.NewLine}" +
                                $"    createproject projectname maxtasksvalue{Environment.NewLine}" +
                                $"    projectlist{Environment.NewLine}" +
                                $"    changeprojectname oldname newname{Environment.NewLine}" +
                                $"    removeproject projectname{Environment.NewLine}" +
                                $"    addtasktoproject projectname taskname tasklevel{Environment.NewLine}" +
                                $"    addusertotask projectname taskname username{Environment.NewLine}" +
                                $"    removeuserfromtask projectname taskname username{Environment.NewLine}" +
                                $"    changetaskstatus projectname taskname newstatus{Environment.NewLine}" +
                                $"    tasklist projectname{Environment.NewLine}" +
                                $"    groupedtasklist projectname{Environment.NewLine}" +
                                $"    removetaskfromproject projectname taskname{Environment.NewLine}" +
                                $"    addsubtasktoepictask projectname taskname subtaskname subtasklevel{Environment.NewLine}" +
                                $"    removesubtaskfromepictask projectname taskname subtaskname{Environment.NewLine}" +
                                $"    changeepictasksubtaskstatus projectname taskname subtaskname newstatus.{Environment.NewLine}" +
                                $"    epictasksubtasklist projectname taskname.{Environment.NewLine}" +
                                $"    epictaskgroupedsubtasklist projectname taskname.{Environment.NewLine}" +
                                $"    addusertoepictasksubtask projectname taskname username.{Environment.NewLine}" +
                                $"    removeuserfromepictasksubtask projectname taskname username.{Environment.NewLine}" +
                                $"You should separate all arguments with space. Names should not contain ones.");
        }

        /// <summary>
        /// Check name's correctness.
        /// </summary>
        /// <param name="nameToCheck"></param>
        /// <returns> True - name is correct, false otherwise. </returns>
        static bool CheckName(string nameToCheck)
        {
            return nameToCheck.All(x => ('a' <= x && x <= 'z') || ('A' <= x && x <= 'Z') || ('0' <= x && x <= '9'));
        }
        static bool CheckInputTaskLevel(string inputLevel)
        {
            return inputLevel switch
            {
                "epic" => true,
                "story" => true,
                "task" => true,
                "bug" => true,
                _ => false
            };
        }
        /// <summary>
        /// Check if string is a correct task status.
        /// </summary>
        /// <param name="inputStatus"></param>
        /// <returns></returns>
        static bool CheckInputTaskStatus(string inputStatus)
        {
            return inputStatus switch
            {
                "opened" => true,
                "workinprogress" => true,
                "completed" => true,
                _ => false
            };
        }
        /// <summary>
        /// COnverts string to taskstatus.
        /// </summary>
        /// <param name="statusString"></param>
        /// <returns></returns>
        static Task.Status StringToTaskStatus(string statusString)
        {
            return statusString switch
            {
                "open" => Task.Status.Opened,
                "workinprogress" => Task.Status.WorkInProgress,
                "completed" => Task.Status.Completed,
                _ => throw new ArgumentException("taskstatus is invalid.")
            };
        }
        /// <summary>
        /// Traslates string into TaskLevel type.
        /// </summary>
        /// <param name="taskLevel"></param>
        /// <returns></returns>
        static Project.TaskLevel StringToTaskLevel(string taskLevel)
        {
            return taskLevel switch
            {
                "epic" => Project.TaskLevel.Epic,
                "story" => Project.TaskLevel.Story,
                "task" => Project.TaskLevel.Task,
                "bug" => Project.TaskLevel.Bug,
                _ => throw new ArgumentException("tasklevel is invalid")
            };
        }
        /// <summary>
        /// Check if command was entered correctly. 
        /// </summary>
        /// <param name="splittedCommand"></param>
        static void CommandCheck(string[] splittedCommand)
        {
            // Фукция довольно длинная, но она не несет сложной смысловой нагрузки (только проверка ввода). Упс, кажется я изменил язык комментариев, я исправлюсь!
            switch (splittedCommand[0])
            {
                case "createuser":
                    if (splittedCommand.Length != 2)
                        throw new ArgumentException("createuser should contain 1 argument - username.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("username does not match naming rules.");
                    break;
                case "userlist":
                    if (splittedCommand.Length != 1)
                        throw new ArgumentException("userlist should contain no arguments.");
                    break;
                case "removeuser":
                    if (splittedCommand.Length != 2)
                        throw new ArgumentException("removeuser should contain 1 argument - username.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("username does not match naming rules.");
                    break;
                case "createproject":
                    if (splittedCommand.Length != 3)
                        throw new ArgumentException("createproject should contain 2 arguments - project name and maximum number of tasks.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!int.TryParse(splittedCommand[2], out int n) || !(0 < n && n < 100))
                        throw new ArgumentException("maximum number of tasks must belong to [1,99].");
                    break;
                case "projectlist":
                    if (splittedCommand.Length != 1)
                        throw new ArgumentException("projectlist should contain no arguments.");
                    break;
                case "changeprojectname":
                    if (splittedCommand.Length != 3)
                        throw new ArgumentException("changeprojectname should contain 2 arguments - old project name and desired new project name.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("old project name does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("new project name does not match naming rules.");
                    break;
                case "removeproject":
                    if (splittedCommand.Length != 2)
                        throw new ArgumentException("removeproject should contain 1 argument - name of project to remove.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    break;
                case "addtasktoproject":
                    if (splittedCommand.Length != 4)
                        throw new ArgumentException("addtasktoproject should contain 3 arguments - project name, task name, level of the task.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    if (!CheckInputTaskLevel(splittedCommand[3]))
                        throw new ArgumentException("task level is invalid.");
                    break;
                case "addusertotask":
                    if (splittedCommand.Length != 4)
                        throw new ArgumentException("addusertotask should contain 3 arguments - project name, task name, name of user to add.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    if (!CheckName(splittedCommand[3]))
                        throw new ArgumentException("username does not match naming rules.");
                    break;
                case "removeuserfromtask":
                    if (splittedCommand.Length != 4)
                        throw new ArgumentException("removeuserfromtask should contain 3 arguments - project name, task name, name of user to remove.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    if (!CheckName(splittedCommand[3]))
                        throw new ArgumentException("username does not match naming rules.");
                    break;
                case "changetaskstatus":
                    if (splittedCommand.Length != 4)
                        throw new ArgumentException("changetaskstatus should contain 3 arguments - project name, task name, name of new status.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    if (!CheckInputTaskStatus(splittedCommand[3]))
                        throw new ArgumentException("taskstatus is invalid.");
                    break;
                case "tasklist":
                    if (splittedCommand.Length != 2)
                        throw new ArgumentException("tasklist should contain 1 argument - project name.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    break;
                case "groupedtasklist":
                    if (splittedCommand.Length != 2)
                        throw new ArgumentException("groupedtasklist should contain 1 argument - project name.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    break;
                case "removetaskfromproject":
                    if (splittedCommand.Length != 3)
                        throw new ArgumentException("removetaskfromproject should contain 2 arguments - project name to remove task from, task name.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    break;
                case "addsubtasktoepictask":
                    if (splittedCommand.Length != 5)
                        throw new ArgumentException("addsubtasktoepictask should contain 4 arguments - project name, epic task name, subtask name, level of subtask.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    if (!CheckName(splittedCommand[3]))
                        throw new ArgumentException("subtaskname does not match naming rules.");
                    if (!CheckInputTaskLevel(splittedCommand[4]))
                        throw new ArgumentException("tasklevel is invalid.");
                    break;
                case "removesubtaskfromepictask":
                    if (splittedCommand.Length != 4)
                        throw new ArgumentException("removesubtaskfromepictask should contain 3 arguments - project name, epic task name, subtask name.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    if (!CheckName(splittedCommand[3]))
                        throw new ArgumentException("subtaskname does not match naming rules.");
                    break;
                case "changeepictasksubtaskstatus":
                    if (splittedCommand.Length != 5)
                        throw new ArgumentException("changeepictasksubtaskstatus should contain 4 arguments - project name, epic task name, subtask name, newstatus.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    if (!CheckName(splittedCommand[3]))
                        throw new ArgumentException("subtaskname does not match naming rules.");
                    if (!CheckInputTaskStatus(splittedCommand[4]))
                        throw new ArgumentException("taskstatus is invalid.");
                    break;
                case "epictasksubtasklist":
                    if (splittedCommand.Length != 3)
                        throw new ArgumentException("epictasksubtasklist should contain 2 arguments - project name, epic task name.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    break;
                case "epictaskgroupedsubtasklist":
                    if (splittedCommand.Length != 3)
                        throw new ArgumentException("epictaskgroupedsubtasklist should contain 2 arguments - project name, epic task name.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    break;
                case "addusertoepictasksubtask":
                    if (splittedCommand.Length != 5)
                        throw new ArgumentException("addusertoepictasksubtask should contain 3 arguments - project name, epic task name, subtask name, username.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    if (!CheckName(splittedCommand[3]))
                        throw new ArgumentException("subtaskname does not match naming rules.");
                    if (!CheckName(splittedCommand[4]))
                        throw new ArgumentException("username does not match naming rules.");
                    break;
                case "removeuserfromepictasksubtask":
                    if (splittedCommand.Length != 5)
                        throw new ArgumentException("addusertoepictasksubtask should contain 4 arguments - project name, epic task name, subtask name, username.");
                    if (!CheckName(splittedCommand[1]))
                        throw new ArgumentException("projectname does not match naming rules.");
                    if (!CheckName(splittedCommand[2]))
                        throw new ArgumentException("taskname does not match naming rules.");
                    if (!CheckName(splittedCommand[3]))
                        throw new ArgumentException("subtaskname does not match naming rules.");
                    if (!CheckName(splittedCommand[4]))
                        throw new ArgumentException("username does not match naming rules.");
                    break;
            }
        }
    }
}
