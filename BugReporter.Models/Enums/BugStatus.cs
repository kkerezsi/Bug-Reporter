using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Enums
{
    [Serializable]
    public enum BugStatus
    {
        ToDo,
        InProgress,
        ReadyForQa
    }
}
