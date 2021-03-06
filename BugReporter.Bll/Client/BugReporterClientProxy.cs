using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BugReporter.Bll.Server.Components;
using BugReporter.Models.Networking.Response;
using BugReporter.Models.Models;
using BugReporter.Models.Networking.RequestObjects;
using BugReporter.Models.Networking.Utils;
using BugReporter.Models.Networking.Request;
using BugReporter.Models.Exceptions;

namespace Server.Components
{
    public class BugReporterClientProxy : IBugReporterServer
    {
        private string _host;
        private int _port;
        private IBugReporterObserver _client;
        private NetworkStream _networkStream;
        private IFormatter _formatter;
        private TcpClient _connection;
        private Queue<Response> _responses;
        private volatile bool _finished;
        private EventWaitHandle _waitHandle;

        public BugReporterClientProxy(string host, int port) : base()
        {
            this._host = host;
            this._port = port;
            _responses = new Queue<Response>();
        }

        public virtual User Login(User user, IBugReporterObserver client)
        {
            InitializeConnection();

            UserInfo uInfo = UserConverter.ConvertUserToUserInfo(user);
            SendRequest(new LoginRequest(uInfo));
            Response response = ReadResponse();

            if (response is OkLoginResponse)
            {
                this._client = client;
                return ((OkLoginResponse)response).User;
            }

            HandleErrorResponse(response);

            return user;
        }

        public virtual void Logout(User user, IBugReporterObserver client)
        {
            UserInfo udto = UserConverter.ConvertUserToUserInfo(user);
            SendRequest(new LogoutRequest(udto));
            Response response = ReadResponse();
            CloseConnection();
            HandleErrorResponse(response);
        }

        private void CloseConnection()
        {
            _finished = true;

            try
            {
                _networkStream.Close();
                _connection.Close();
                _waitHandle.Close();
                _client = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void SendRequest(Request request)
        {
            try
            {
                _formatter.Serialize(_networkStream, request);
                _networkStream.Flush();
            }
            catch (Exception e)
            {
                throw new ConnectionException("Error sending object " + e);
            }
        }

        private Response ReadResponse()
        {
            Response response = null;

            try
            {
                _waitHandle.WaitOne();

                lock (_responses)
                {
                    response = _responses.Dequeue();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return response;
        }

        private void InitializeConnection()
        {
            try
            {
                _connection = new TcpClient();
                _connection.Connect(_host, _port);

                _networkStream = _connection.GetStream();
                _formatter = new BinaryFormatter();
                _finished = false;
                _waitHandle = new AutoResetEvent(false);
                StartReader();
            }


            catch (Exception e)
            {
                throw new ConnectionException(e.Message + " ------------------- " + e.StackTrace);
            }
        }

        private void StartReader()
        {
            Thread tw = new Thread(Run);
            tw.Start();
        }

        public virtual void Run()
        {
            while (!_finished)
            {
                try
                {
                    object response = _formatter.Deserialize(_networkStream);

                    if (response is ReportsResponse)
                    {
                        _client.UpdateReports(((ReportsResponse)response).ReportList);
                    }
                    else
                    {
                        lock (_responses)
                        {
                            _responses.Enqueue((Response)response);
                        }

                        _waitHandle.Set();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Reading error " + e);
                }
            }
        }

        public ReportModel GetReports(IBugReporterObserver client)
        {
            SendRequest(new ReportsRequest());

            return new ReportModel() { };
        }

        public void SaveReports(ReportModel reportModel)
        {
            SendRequest(new SaveReportRequest() { Reports = reportModel });
        }

        public UserList GetUserList(int? projectId = null)
        {
            SendRequest(new UserListRequest());
            Response response = ReadResponse();

            if (response is UserListResponse)
            {
                UserListResponse result = (UserListResponse)response;

                return result.UserList;
            }

            HandleErrorResponse(response);

            return new UserList();
        }

        private void HandleErrorResponse(Response response)
        {
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                CloseConnection();
                throw new ConnectionException(err.Message);
            }
        }
    }
}