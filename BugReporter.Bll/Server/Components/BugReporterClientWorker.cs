using BugReporter.Models.Enums;
using BugReporter.Models.Exceptions;
using BugReporter.Models.Models;
using BugReporter.Models.Networking.Request;
using BugReporter.Models.Networking.RequestObjects;
using BugReporter.Models.Networking.Response;
using BugReporter.Models.Networking.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BugReporter.Bll.Server.Components
{
    public class BugReporterClientWorker :  RequestHandlers, IBugReporterObserver
    {
        #region ctor

        public BugReporterClientWorker(IBugReporterServer server, TcpClient client) : base(server,client)
        {
        }

        #endregion

        public void Run()
        {
            while (_connected)
            {
                try
                {
                    object request = _formatter.Deserialize(_stream);
                    object response = HandleRequest((Request)request);
                    if (response != null)
                    {
                        SendResponse((Response)response);
                    }

                    _stream.Flush();
                }
                catch (Exception e)
                {
                    _connected = false;

                    _server.Logout(CurrentUser, this);
                    Console.WriteLine(CurrentUser.Username + " User process failed.");
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            try
            {
                _stream.Close();
                _connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e);
            }
        }

        private Response HandleRequest(Request request)
        {
            Response response = null;
            if (request is LoginRequest)
            {
                return HandleLoginRequest(request, this);
            }

            if (request is LogoutRequest)
            {
                return HandleLogoutRequest(request, this);
            }

            if(request is ReportsRequest)
            {
                return HandleReportsRequest(request, this);
            }

            if (request is SaveReportRequest)
            {
                return HandleSaveReportRequest(request, this);
            }

            if (request is UserListRequest)
            {
                return HandleUserListRequest(request, this);
            }

            return response;
        }

        public void UpdateReports(ReportModel reportModel)
        {
            Console.WriteLine("Reports update");

            var reports = _server.GetReports(this);
            try
            {
                SendResponse(new ReportsResponse() { ReportList = reports });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
