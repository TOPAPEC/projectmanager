using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectControlClasses
{
    [Serializable]
    public class EpicTask : Task, IAssignable
    {
        readonly List<Task> subTaskList = new List<Task>();
        public EpicTask(string taskName) : base(taskName) { }
        public List<string> GetExecutors()
        {
            return new List<string>();
        }
        public void AddUser(string userName)
        {
            throw new ArgumentException("Epic tasks have no executors.");
        }
        public void RemoveUser(string userName)
        {
            throw new ArgumentException("Epic tasks have no executors.");
        }
        const int maxNumberOfSubtasks = 15;
        /// <summary>
        /// Adds task to subtasklist.
        /// </summary>
        /// <param name="taskName"> Name of new task. </param>
        public void AddTask(string taskName, Project.TaskLevel typeOfTask)
        {
            if (subTaskList.Count >= maxNumberOfSubtasks)
                throw new OverflowException("Maximum number of subtasks reached.");
            if (subTaskList.Any(x => x.TaskName == taskName))
                throw new ArgumentException($"Subtask with name {taskName} already exists.");
            switch (typeOfTask)
            {
                case Project.TaskLevel.Story:
                    subTaskList.Add(new StoryTask(taskName));
                    break;
                case Project.TaskLevel.Task:
                    subTaskList.Add(new SimpleTask(taskName));
                    break;
                default:
                    throw new ArgumentException("Epictask can only contain stories and tasks.");
            }
        }
        /// <summary>
        /// Removes task from subtasklist.
        /// </summary>
        /// <param name="taskName"> Name of task to remove. </param>
        public void RemoveTask(string taskName)
        {
            var taskToRemove = subTaskList.Find(x => x.TaskName == taskName);
            if (taskToRemove == default(Task))
                throw new ArgumentException($"No subtask of {TaskName} with name {taskName}.");
            subTaskList.Remove(taskToRemove);
        }
        /// <summary>
        /// Converts list of names to string of names.
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
        /// Returns list of subtasks.
        /// </summary>
        /// <returns></returns>
        public List<string> GetSubtaskList()
        {
            if (subTaskList.Count <= 0)
                throw new NullReferenceException("Epictask is empty.");
            return subTaskList.Select((x, index) => $"{index + 1}. {x.TaskName} {x.CreationDate} [{ExecutorListToString(((IAssignable)x).GetExecutors())}] {x.TaskStatus}").ToList();
        }
        /// <summary>
        /// Changes subtask's status.
        /// </summary>
        /// <param name="subtaskname"></param>
        /// <param name="newStatus"></param>
        public void ChangeTaskStatus(string subtaskname, Task.Status newStatus)
        {
            Task taskToChangeStatus;
            if ((taskToChangeStatus = subTaskList.Find(x => x.TaskName == subtaskname)) == default(Task))
                throw new ArgumentException($"Subtask with name {subtaskname} not found.");
            taskToChangeStatus.TaskStatus = newStatus;
        }
        /// <summary>
        /// Assigns user to subtask.
        /// </summary>
        /// <param name="subtaskName"></param>
        /// <param name="userName"></param>
        public void AssignUserToSubtask(string subtaskName, string userName)
        {
            Task taskToAddUserTo;
            if ((taskToAddUserTo = subTaskList.Find(x => x.TaskName == subtaskName)) == default(Task))
                throw new ArgumentException($"Subtask with name {subtaskName} not found.");
            IAssignable subtaskToIAssignable = (IAssignable)taskToAddUserTo;
            subtaskToIAssignable.AddUser(userName);
        }
        /// <summary>
        /// Removes user from subtask.
        /// </summary>
        /// <param name="subtaskName"></param>
        /// <param name="userName"></param>
        public void RemoveUserFromSubtask(string subtaskName, string userName)
        {
            Task subtaskToRemoveUserFrom;
            if ((subtaskToRemoveUserFrom = subTaskList.Find(x => x.TaskName == subtaskName)) == default(Task))
                throw new ArgumentException($"Subtask with name {subtaskName} not found.");
            IAssignable subtaskToIAssignable = (IAssignable)subtaskToRemoveUserFrom;
            subtaskToIAssignable.RemoveUser(userName);
        }

        /// <summary>
        /// Returns list of string containing task ordered by their status.
        /// </summary>
        /// <returns></returns>
        public List<string> GetOrderedByStatusSubtaskList()
        {
            if (subTaskList.Count <= 0)
                throw new NullReferenceException("Epictask is empty.");
            List<string> returnList = new List<string>();
            returnList.Add("Opened:");
            returnList.AddRange(subTaskList.FindAll(x => x.TaskStatus == Status.Opened).Select((x, index) => $"{index}. {x.TaskName} [{ExecutorListToString(((IAssignable)x).GetExecutors())}]"));
            returnList.Add("Work in progress:");
            returnList.AddRange(subTaskList.FindAll(x => x.TaskStatus == Status.WorkInProgress).Select((x, index) => $"{index}. {x.TaskName} [{ExecutorListToString(((IAssignable)x).GetExecutors())}]"));
            returnList.Add("Completed:");
            returnList.AddRange(subTaskList.FindAll(x => x.TaskStatus == Status.Completed).Select((x, index) => $"{index}. {x.TaskName} [{ExecutorListToString(((IAssignable)x).GetExecutors())}]"));
            return returnList;
        }
    }
}
