using BugReporter.Bll.Server.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugReporter.Models.Models;
using BugReporter.Models.Enums;

namespace BugReporter.Bll.Server.Repositories
{
    public class TextUserRepository : IUserRepository
    {
        private List<User> _users;

        public TextUserRepository()
        {
            _users = new List<User>();

            Initialize();
        }

        private void Initialize()
        {
            _users.Add(new User() { Username = "Developer", Password = "Developer", Type = UserType.Developer });
            _users.Add(new User() { Username = "Tester", Password = "Tester", Type = UserType.Tester });
            _users.Add(new User() { Username = "Tester2", Password = "Tester2", Type = UserType.Tester });
        }

        public bool VerifyUser(User user)
        {
            return _users.Any(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password));
        }

        public User GetUser(User user)
        {
            return _users.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password)).FirstOrDefault();
        }
    }
}
