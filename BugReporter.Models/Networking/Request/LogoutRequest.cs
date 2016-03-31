using BugReporter.Models.Networking.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Networking.Request
{
    [Serializable()]
    public class LogoutRequest : Request
    {
        public LogoutRequest(UserInfo uInfo)
        {
            User = uInfo;
        }

        public UserInfo User { get; set; }
    }
}
