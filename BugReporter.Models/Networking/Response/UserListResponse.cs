using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Networking.Response
{
    [Serializable]
    public class UserListResponse : Response
    {
        public UserList UserList { get; set; }
    }
}
