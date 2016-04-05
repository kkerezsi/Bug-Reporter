using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Bll.Server.Components
{
    public interface IUserRepository
    {
        bool VerifyUser(User user);
        User GetUser(User user);
        UserList GetAllUsers();
        UserList GetUsersByProject(int projectId);
    }
}
