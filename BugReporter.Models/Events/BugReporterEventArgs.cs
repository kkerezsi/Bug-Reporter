using BugReporter.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Models.Events
{
    public class BugReporterEventArgs : EventArgs
    {
        private readonly BugReporterUserEvents userEvent;
        private readonly Object data;

        public BugReporterEventArgs(BugReporterUserEvents userEvent, object data)
        {
            this.userEvent = userEvent;
            this.data = data;
        }

        public BugReporterUserEvents UserEventType
        {
            get { return userEvent; }
        }

        public object Data
        {
            get { return data; }
        }
    }
}
