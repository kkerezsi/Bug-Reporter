using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Networking.Request
{
    [Serializable]
    public class SaveReportRequest : Request
    {
        public ReportModel Reports { get; set; }
    }
}
