using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Models
{
    [Serializable]
    public class ReportModel
    {
        public List<Project> Projects { get; set; }

        public ReportModel()
        {
            Projects = new List<Project>();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
