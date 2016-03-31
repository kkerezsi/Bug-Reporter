using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Networking.Response
{
    [Serializable]
    public class SaveReportResponse : Response
    {
        public int Status { get; set; }
        public ReportModel Reports { get; set; }
    }
}
