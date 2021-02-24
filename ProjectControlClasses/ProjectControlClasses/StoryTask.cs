using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectControlClasses
{
    [Serializable]
    public class StoryTask : Task, IAssignable
    {
        public StoryTask(string taskName) : base(taskName) { }

        public List<User> ListOfExecutors = new List<User>();
        const int maxNumberOfExecutors = 10;
        public List<string> GetExecutors()
        {
            return new List<string>(new string[ListOfExecutors.Count]).Select((x, index) => ListOfExecutors[index].UserName).ToList();
        }
        /// <summary>
        /// Adds new user to list of executors.
        /// </summary>
        /// <param name="userName"></param>
        public void AddUser(string userName)
        {
            if (ListOfExecutors.Count >= 10)
                throw new OverflowException("Story task cannot contain more than 10 executors.");
            if (ListOfExecutors.Any(x => x.UserName == userName))
                throw new ArgumentException($"User {userName} is already working on this story.");
            ListOfExecutors.Add(new User(userName));
        }
        /// <summary>
        /// Removes users from list of executors.
        /// </summary>
        /// <param name="userName"></param>
        public void RemoveUser(string userName)
        {
            if (!ListOfExecutors.Any(x => x.UserName == userName))
                throw new ArgumentException($"User {userName} doesn't work on this story.");
            ListOfExecutors.Remove(ListOfExecutors.Find(x => x.UserName == userName));
        }
    }
}
