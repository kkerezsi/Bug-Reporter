using BugReporter.Bll.Server.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BugReporter.Bll.Server.Versions
{
    public class SerialBugReporterServer : ConcurrentServer
    {
        private IBugReporterServer _server;
        private BugReporterClientWorker _worker;

        public SerialBugReporterServer(string host, int port, IBugReporterServer server) : base(host, port)
        {
            _server = server;
            Console.WriteLine("SerialBugReporterServer...");
        }

        protected override Thread CreateWorker(TcpClient client)
        {
            _worker = new BugReporterClientWorker(_server, client);
            return new Thread(new ThreadStart(_worker.Run));
        }
    }
}
