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
    public class BugReporterClientWorker : IBugReporterObserver
    {
        private TcpClient _connection { get; set; }
        private IBugReporterServer _server { get; set; }

        private NetworkStream _stream;
        private IFormatter _formatter;
        private volatile bool _connected;

        public BugReporterClientWorker(IBugReporterServer server, TcpClient client)
        {
            _server = server;
            _connection = client;
            try
            {
                _stream = _connection.GetStream();
                _formatter = new BinaryFormatter();
                _connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

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

                    Console.WriteLine(e.StackTrace);
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
                Console.WriteLine("Login request ...");
                LoginRequest loginRequest = (LoginRequest)request;
                UserInfo uInfo = loginRequest.User;
                User user = UserConverter.ConvertUserInfoToUser(uInfo);
                try
                {
                    User userResponse;
                    lock (_server)
                    {
                        userResponse = _server.Login(user, this);
                    }

                    return new OkLoginResponse() { User = userResponse };
                }
                catch (ConnectionException e)
                {
                    _connected = false;
                    return new ErrorResponse() { Message = e.Message };
                }
            }

            if (request is LogoutRequest)
            {
                Console.WriteLine("Logout request");
                LogoutRequest logoutRequest = (LogoutRequest)request;
                UserInfo uInfo = logoutRequest.User;
                User user = UserConverter.ConvertUserInfoToUser(uInfo);
                try
                {
                    lock (_server)
                    {
                        _server.Logout(user, this);
                    }

                    _connected = false;
                    return new OkLoginResponse();
                }
                catch (ConnectionException e)
                {
                    return new ErrorResponse() { Message = e.Message };
                }
            }

            if(request is ReportsRequest)
            {
                try
                {
                    ReportModel reports = null;

                    lock (_server)
                    {
                        reports = _server.GetReports(this);
                    }

                    return new ReportsResponse() { ReportList = reports } ;
                }
                catch (ConnectionException e)
                {
                    return new ErrorResponse() { Message = e.Message };
                }
            }

            if (request is SaveReportRequest)
            {
                try
                {
                    int statusCode = 0;

                    lock (_server)
                    {
                        statusCode = _server.SaveReports(((SaveReportRequest)request).Reports);
                    }

                    return new SaveReportResponse() { Status = (int)StatusCodes.OK, Reports = ((SaveReportRequest)request).Reports };
                }
                catch (ConnectionException e)
                {
                    return new ErrorResponse() { Message = e.Message };
                }
            }

            return response;
        }

        private void SendResponse(Response response)
        {
            Console.WriteLine("sending response " + response);
            _formatter.Serialize(_stream, response);
            _stream.Flush();
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
