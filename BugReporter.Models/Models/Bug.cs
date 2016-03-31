using BugReporter.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Models
{
    [Serializable]
    public class Bug
    {
        public User Reporter { get; set; }
        public User AssignedTo { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public BugStatus Status { get; set; }
    }
}
