using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectControlClasses
{
    [Serializable]
    public class ProjectManager
    {
        /// <summary>
        /// List of users available.
        /// </summary>
        readonly List<User> userList = new List<User>();
        /// <summary>
        /// List of active projects.
        /// </summary>
        readonly List<Project> projectList = new List<Project>();
        /// <summary>
        /// Returns list of active projects.
        /// </summary>
        /// <returns>List of active projects.</returns>
        public List<string> GetProjectList()
        {
            if (projectList.Count <= 0)
                throw new NullReferenceException("No active projects yet.");
            return (new List<string>(new string[projectList.Count])).Select((x, index) => x = $"{index + 1}. {projectList[index].ProjectName} {projectList[index].TaskCount}").ToList();
        }
        /// <summary>
        /// Checks if user with username exists and removes it. Otherwise throws exception.
        /// </summary>
        /// <param name="userName"> Name of user to remove. </param>
        public void RemoveUser(string userName)
        {
            User userToRemove;
            if ((userToRemove = userList.Find(x => x.UserName == userName)) == default(User))
            {
                throw new ArgumentException("No users with such name found.");
            }
            userList.Remove(userToRemove);
        }
        /// <summary>
        /// Adds user if his name is unique.
        /// </summary>
        /// <param name="userName">Desired username.</param>
        public void AddUser(string userName)
        {
            if (userList.Any(x => (x as User).UserName == userName))
            {
                throw new ArgumentException("User with such name already exists.");
            }
            userList.Add(new User(userName));
        }
        /// <summary>
        /// Returns list of usersnames.
        /// </summary>
        /// <returns> Formatted list of users. </returns>
        public List<string> GetUserList()
        {
            if (userList.Count <= 0)
            {
                throw new NullReferenceException("List of users is empty.");
            }
            return (new List<string>(new string[userList.Count])).Select((x, index) => x = $"{index + 1}. {userList[index].UserName}").ToList();
        }
        /// <summary>
        /// Assigns user to task in projectName project.
        /// </summary>
        /// <param name="projectName"> Name of project to assign user to. </param>
        /// <param name="taskName"> Name of task to assign user to. </param>
        /// <param name="userName"> Name of user. </param>
        public void AssignUserToTask(string projectName, string taskName, string userName)
        {
            if (!projectList.Any(x => x.ProjectName == projectName))
                throw new ArgumentException("Invalid project name.");
            if (!userList.Any(x => x.UserName == userName))
                throw new ArgumentException("Invalid username.");
            projectList.Find(x => x.ProjectName == projectName).AssignUserToTask(taskName, userName);
        }
        /// <summary>
        /// Assign user to epic task's subtask.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="taskName"></param>
        /// <param name="subtaskName"></param>
        /// <param name="userName"></param>
        public void AssignUserToSubtask(string projectName, string taskName, string subtaskName, string userName)
        {
            if (!projectList.Any(x => x.ProjectName == projectName))
                throw new ArgumentException("Invalid project name.");
            if (!userList.Any(x => x.UserName == userName))
                throw new ArgumentException("Invalid username.");
            projectList.Find(x => x.ProjectName == projectName).GetEpicTask(taskName).AssignUserToSubtask(subtaskName, userName);
        }
        /// <summary>
        /// Remove user from task in projectName project.
        /// </summary>
        /// <param name="projectName"> Name of project to remove user from. </param>
        /// <param name="taskName"></param>
        /// <param name="userName"></param>
        public void RemoveUserFromTask(string projectName, string taskName, string userName)
        {
            if (!projectList.Any(x => x.ProjectName == projectName))
                throw new ArgumentException("Invalid project name.");
            if (!userList.Any(x => x.UserName == userName))
                throw new ArgumentException("Invalid username.");
            projectList.Find(x => x.ProjectName == projectName).RemoveUserFromTask(taskName, userName);
        }
        public void RemoveUserFromSubtask(string projectName, string taskName, string subtaskName, string userName)
        {
            if (!projectList.Any(x => x.ProjectName == projectName))
                throw new ArgumentException("Invalid project name.");
            if (!userList.Any(x => x.UserName == userName))
                throw new ArgumentException("Invalid username.");
            projectList.Find(x => x.ProjectName == projectName).GetEpicTask(taskName).RemoveUserFromSubtask(taskName, userName);
        }
        /// <summary>
        /// Creates new project.
        /// </summary>
        /// <param name="projectName"> Name of the project. </param>
        /// <param name="maxNumberOfProjects"> Maximum number of active tasks. </param>
        public void CreateProject(string projectName, int maxNumberOfTasks)
        {
            if (projectList.Any(x => x.ProjectName == projectName))
                throw new ArgumentException("Project with such name already exists.");
            projectList.Add(new Project(projectName, maxNumberOfTasks));
        }
        /// <summary>
        /// Checks if project exists and then remove it. 
        /// </summary>
        /// <param name="projectName"> Name of the project to be deleted. </param>
        public void RemoveProject(string projectName)
        {
            var projectToRemove = projectList.Find(x => x.ProjectName == projectName);
            if (projectToRemove == default(Project))
                throw new ArgumentException($"Project with name {projectName} not found.");
            projectList.Remove(projectToRemove);
        }
        /// <summary>
        /// Changes name of the project.
        /// </summary>
        /// <param name="oldName"> Current name. </param>
        /// <param name="newName"> Desired name of the project. </param>
        public void ChangeProjectName(string oldName, string newName)
        {
            var projectToRename = projectList.Find(x => x.ProjectName == oldName);
            if (projectToRename == default(Project))
                throw new ArgumentException($"Project with name {oldName} not found.");
            projectToRename.ProjectName = newName;
        }
        /// <summary>
        /// Returns project with specified name.
        /// </summary>
        /// <param name="projectName"> Project name. </param>
        /// <returns></returns>
        public Project GetProject(string projectName)
        {
            if (!projectList.Any(x => x.ProjectName == projectName))
                throw new ArgumentException("Incorrect projectname.");
            return projectList.Find(x => x.ProjectName == projectName);
        }
    }
}
