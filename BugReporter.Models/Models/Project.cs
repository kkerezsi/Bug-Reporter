using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Models
{
    [Serializable]
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Bug> Bugs { get; set; }

        public Project()
        {
            Bugs = new List<Bug>();
        }
    }
}
