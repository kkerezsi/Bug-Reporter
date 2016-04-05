using BugReporter.Models.Networking.RequestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Networking.Request
{
    [Serializable()]
    public class UserListRequest : Request
    {
        public int? ProjectId { get; set; }
        public UserListRequest(int? projectId = null)
        {
            ProjectId = projectId;
        }
    }
}
