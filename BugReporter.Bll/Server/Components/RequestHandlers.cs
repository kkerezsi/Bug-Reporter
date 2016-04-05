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
using System.Threading.Tasks;

namespace BugReporter.Bll.Server.Components
{
    public class RequestHandlers
    {
        protected TcpClient _connection { get; set; }
        protected IBugReporterServer _server { get; set; }

        protected NetworkStream _stream;
        protected IFormatter _formatter;
        protected volatile bool _connected;

        public User CurrentUser { get; set; }

        public RequestHandlers(IBugReporterServer server, TcpClient client)
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

        public Response HandleLoginRequest(Request request, IBugReporterObserver self)
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
                    userResponse = _server.Login(user, self);
                }

                CurrentUser = userResponse;
                return new OkLoginResponse() { User = userResponse };
            }
            catch (ConnectionException e)
            {
                _connected = false;
                return new ErrorResponse() { Message = e.Message };
            }
        }

        public Response HandleLogoutRequest(Request request, IBugReporterObserver self)
        {
            Console.WriteLine("Logout request");
            LogoutRequest logoutRequest = (LogoutRequest)request;
            UserInfo uInfo = logoutRequest.User;
            User user = UserConverter.ConvertUserInfoToUser(uInfo);
            try
            {
                lock (_server)
                {
                    _server.Logout(user, self);
                }

                _connected = false;
                return new OkLoginResponse();
            }
            catch (ConnectionException e)
            {
                return new ErrorResponse() { Message = e.Message };
            }
        }

        public Response HandleReportsRequest(Request request, IBugReporterObserver self)
        {
            try
            {
                ReportModel reports = null;

                lock (_server)
                {
                    reports = _server.GetReports(self);
                }

                return new ReportsResponse() { ReportList = reports };
            }
            catch (ConnectionException e)
            {
                return new ErrorResponse() { Message = e.Message };
            }
        }

        public Response HandleSaveReportRequest(Request request, IBugReporterObserver self)
        {
            try
            {
                lock (_server)
                {
                    _server.SaveReports(((SaveReportRequest)request).Reports);
                }

                return new SaveReportResponse() { Status = (int)StatusCodes.OK, Reports = ((SaveReportRequest)request).Reports };
            }
            catch (ConnectionException e)
            {
                return new ErrorResponse() { Message = e.Message };
            }
        }

        public Response HandleUserListRequest(Request request, IBugReporterObserver self)
        {
            try
            {
                UserList users = null;

                lock (_server)
                {
                    users = _server.GetUserList(((UserListRequest)request).ProjectId);
                }

                return new UserListResponse() { UserList = users };
            }
            catch (ConnectionException e)
            {
                return new ErrorResponse() { Message = e.Message };
            }
        }

        protected void SendResponse(Response response)
        {
            Console.WriteLine("sending response " + response);
            _formatter.Serialize(_stream, response);
            _stream.Flush();
        }
    }
}
