using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectControlClasses
{
    [Serializable]
    public class Task
    {
        readonly string taskName;
        public string TaskName
        {
            get => taskName;
        }
        protected Task(string taskName) => (this.taskName, this.creationDate, TaskStatus) = (taskName, DateTime.Now, Status.Opened);

        readonly DateTime creationDate;
        public DateTime CreationDate
        {
            get => creationDate;
        }
        /// <summary>
        /// Enum that describes all possible statuses of task.
        /// </summary>
        public enum Status
        {
            Opened = 0,
            WorkInProgress = 1,
            Completed = 2
        }
        public Status TaskStatus { get; set; }
    }
}
