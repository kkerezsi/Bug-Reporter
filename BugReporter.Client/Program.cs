using BugReporter.Bll.Client;
using BugReporter.Bll.Server.Components;
using Server.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugReporter.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IBugReporterServer server = new BugReporterClientProxy("127.0.0.1", 12345);
            BugReporterClientCtrl ctrl = new BugReporterClientCtrl(server);

            Application.Run(new Login(ctrl));
        }
    }
}
