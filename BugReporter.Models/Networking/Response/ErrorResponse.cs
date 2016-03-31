using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Networking.Response
{
    [Serializable]
    public class ErrorResponse : Response
    {
        public string Message { get; set; }
    }
}
