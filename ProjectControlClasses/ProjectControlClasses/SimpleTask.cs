using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectControlClasses
{
    [Serializable]
    public class SimpleTask : Task, IAssignable
    {
        public SimpleTask(string taskName) : base(taskName) { }
        public User Executor { get; set; }
        public List<string> GetExecutors()
        {
            return new List<string>(new string[1]).Select(x => Executor.UserName).ToList();
        }
        /// <summary>
        /// Sets user to executor var if it is empty.
        /// </summary>
        /// <param name="userName"></param>
        public void AddUser(string userName)
        {
            if (Executor != default(User))
                throw new ArgumentException("This task already have executor.");
            Executor = new User(userName);
        }
        /// <summary>
        /// Removes user from executor var.
        /// </summary>
        /// <param name="userName"></param>
        public void RemoveUser(string userName)
        {
            if (!(Executor.UserName != userName))
                throw new ArgumentException($"{userName} not found in this task.");
            Executor = default(User);
        }
    }
}
