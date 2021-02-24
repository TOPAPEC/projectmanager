using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectControlClasses
{
    public interface IAssignable
    {
        void AddUser(string userName);
        void RemoveUser(string userName);
        List<string> GetExecutors();
    }
}
