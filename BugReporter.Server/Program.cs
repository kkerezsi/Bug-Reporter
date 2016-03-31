using BugReporter.Bll.Repositories;
using BugReporter.Bll.Repositories.Contracts;
using BugReporter.Bll.Server.Components;
using BugReporter.Bll.Server.Repositories;
using BugReporter.Bll.Server.Versions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting server..");
            IUserRepository userRepo = new TextUserRepository();
            IReportsRepository reportsRepo = new TextReportsRepository();
            ServerImplementation sImplementation = new ServerImplementation(userRepo, reportsRepo);
            SerialBugReporterServer server = new SerialBugReporterServer("127.0.0.1", 12345, sImplementation);

            server.Start();
        }
    }
}
