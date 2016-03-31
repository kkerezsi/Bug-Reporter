using BugReporter.Models.Models;
using BugReporter.Models.Networking.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Networking.Utils
{
    public static class UserConverter
    {
        public static User ConvertUserInfoToUser(UserInfo uInfo)
        {
            return new User()
            {
                Username = uInfo.Username,
                Password = uInfo.Password
            };
        }

        public static UserInfo ConvertUserToUserInfo(User user)
        {
            return new UserInfo()
            {
                Username = user.Username,
                Password = user.Password
            };
        }
    }
}
