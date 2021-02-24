using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectControlClasses
{
    [Serializable]
    public class User
    {
        readonly string userName;
        public string UserName
        {
            get => userName;
        }
        public User(string name) => userName = name;
    }
}
