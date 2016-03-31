using BugReporter.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Models
{
    [Serializable]
    public class User
    {
        public User()
        {
            ProjectLists = new List<int>();
        }

        public string Username { get; set; }
        public List<int> ProjectLists { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
    }
}
