using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProjectControlClasses
{
    [Serializable]
    public class Project
    {
        public string ProjectName { get; set; }
        public int TaskCount
        {
            get => taskList.Count;
        }

        readonly List<Task> taskList = new List<Task>();
        /// <summary>
        /// Stores levels of tasks.
        /// </summary>
        public enum TaskLevel
        {
            Epic,
            Story,
            Task,
            Bug
        }
        readonly int maxNumberOfTasks;
        public Project(string projectName, int maxNumberOfProjects)
        {
            (this.ProjectName, this.maxNumberOfTasks) = (projectName, maxNumberOfProjects);
        }

        /// <summary>
        /// Creates new task if its name is unique.
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="taskLevel"> Type (epic, story, etc.) of new task. </param>
        public void CreateNewTask(string taskName, TaskLevel taskLevel)
        {
            if (taskList.Any(x => x.TaskName == taskName))
                throw new ArgumentException("Task with such name already exists.");
            if (TaskCount >= maxNumberOfTasks)
                throw new OverflowException("Maximum number of tasks reached.");
            switch (taskLevel)
            {
                case TaskLevel.Epic:
                    taskList.Add(new EpicTask(taskName));
                    break;
                case TaskLevel.Story:
                    taskList.Add(new StoryTask(taskName));
                    break;
                case TaskLevel.Task:
                    taskList.Add(new SimpleTask(taskName));
                    break;
                case TaskLevel.Bug:
                    taskList.Add(new BugTask(taskName));
                    break;
            }
        }
        /// <summary>
        /// Adds executors to task.
        /// </summary>
        /// <param name="taskName"> Name of task. </param>
        /// <param name="userName"> User to be added. </param>
        public void AssignUserToTask(string taskName, string userName)
        {
            Task taskToAddUserTo;
            if ((taskToAddUserTo = taskList.Find(x => x.TaskName == taskName)) == default(Task))
                throw new ArgumentException($"Task with name {taskName} not found.");
            IAssignable taskToIAssignable = (IAssignable)taskToAddUserTo;
            taskToIAssignable.AddUser(userName);
        }
        /// <summary>
        /// Removes specified users from executers of task taskName.
        /// </summary>
        /// <param name="taskName"> Name of the task. </param>
        /// <param name="userName"> User to remove. </param>
        public void RemoveUserFromTask(string taskName, string userName)
        {
            Task taskToRemoveUserFrom;
            if ((taskToRemoveUserFrom = taskList.Find(x => x.TaskName == taskName)) == default(Task))
                throw new ArgumentException($"Task with name {taskName} not found.");
            ((IAssignable)taskToRemoveUserFrom).RemoveUser(userName);
        }
        /// <summary>
        /// Changes specified task's status. 
        /// </summary>
        /// <param name="taskName">Task name. </param>
        /// <param name="newStatus"> New status of task. </param>
        public void ChangeTaskStatus(string taskName, Task.Status newStatus)
        {
            Task taskToChangeStatus;
            if ((taskToChangeStatus = taskList.Find(x => x.TaskName == taskName)) == default(Task))
                throw new ArgumentException($"Task with name {taskName} not found.");
            taskToChangeStatus.TaskStatus = newStatus;
        }
        /// <summary>
        /// Returns array with (name, date, executors, status) of all tasks.
        /// </summary>
        /// <returns> Tasks' (name, date, executors, status) array. </returns>
        public List<string> GetTaskList()
        {
            if (taskList.Count <= 0)
                throw new NullReferenceException("Project is empty.");
            return taskList.Select((x, index) => $"{index + 1}. {x.TaskName} {x.CreationDate} [{ExecutorListToString(((IAssignable)x).GetExecutors())}] {x.TaskStatus}").ToList();
        }
        /// <summary>
        /// Removes task from the project if it is there.
        /// </summary>
        /// <param name="taskName"> Name of the task to remove. </param>
        public void RemoveTask(string taskName)
        {
            Task elementToRemove;
            if ((elementToRemove = taskList.Find(x => x.TaskName == taskName)) == default(Task))
                throw new ArgumentException($"Task with name {taskName} is not found.");
            taskList.Remove(elementToRemove);
        }
        /// <summary>
        /// Converts List of usernames to one string.
        /// </summary>
        /// <param name="executors"></param>
        /// <returns></returns>
        string ExecutorListToString(List<string> executors)
        {
            if (executors.Count == 0)
                return "";
            StringBuilder retString = new StringBuilder();
            foreach (var exec in executors)
                retString.Append($"{exec},");
            return retString.ToString();
        }
        /// <summary>
        /// Returns list of tasks, ordered by status.
        /// </summary>
        /// <returns> List of task names sorted by status. </returns>
        public List<string> GetOrderedByStatusTaskList()
        {
            List<string> returnList = new List<string>();
            // Using linq to reduce code size.
            returnList.Add("Opened:");
            returnList.AddRange(taskList.FindAll(x => x.TaskStatus == Task.Status.Opened).Select((x, index) => $"{index}. {x.TaskName} [{ExecutorListToString(((IAssignable)x).GetExecutors())}]"));
            returnList.Add("Work in progress:");
            returnList.AddRange(taskList.FindAll(x => x.TaskStatus == Task.Status.WorkInProgress).Select((x, index) => $"{index}. {x.TaskName} [{ExecutorListToString(((IAssignable)x).GetExecutors())}]"));
            returnList.Add("Completed:");
            returnList.AddRange(taskList.FindAll(x => x.TaskStatus == Task.Status.Completed).Select((x, index) => $"{index}. {x.TaskName} [{ExecutorListToString(((IAssignable)x).GetExecutors())}]"));
            return returnList;
        }
        /// <summary>
        /// Adds subtask to taskName epic task.
        /// </summary>
        /// <param name="taskName"> Name of epic task. </param>
        /// <param name="subtaskName"> Name of new task. </param>
        public void AddTaskToEpicTask(string taskName, string subtaskName, TaskLevel subtaskType)
        {
            var taskToAddTo = taskList.Find(x => x.TaskName == taskName);
            if (taskToAddTo == default(Task) || taskToAddTo.GetType() != typeof(EpicTask))
                throw new ArgumentException($"{taskName} does not exist or not an epic task.");
            (taskToAddTo as EpicTask).AddTask(subtaskName, subtaskType);
        }
        /// <summary>
        /// Removes specified subtask from epic task.
        /// </summary>
        /// <param name="taskName"> Epic task name. </param>
        /// <param name="subtaskName"> Subtask name. </param>
        public void RemoveSubtaskFromEpicTask(string taskName, string subtaskName)
        {
            var taskToRemoveFrom = taskList.Find(x => x.TaskName == taskName);
            if (taskToRemoveFrom == default(Task) || taskToRemoveFrom.GetType() != typeof(EpicTask))
                throw new ArgumentException($"{taskName} does not exist or not an epic task.");
            (taskToRemoveFrom as EpicTask).RemoveTask(subtaskName);
        }
        /// <summary>
        /// Finds and returns subtask by its name.
        /// </summary>
        /// <param name="taskName"></param>
        /// <returns></returns>
        public EpicTask GetEpicTask(string taskName)
        {
            var taskToReturn = taskList.Find(x => x.TaskName == taskName);
            if (taskToReturn == default(Task))
                throw new ArgumentException("Task with such name was not found.");
            if (!(taskToReturn is EpicTask))
                throw new ArgumentException("Task with such name is not an epic task.");
            return (EpicTask)taskToReturn;
        }

    }
}
