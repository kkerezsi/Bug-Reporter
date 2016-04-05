using BugReporter.Bll.Server.Components;
using BugReporter.Models.Enums;
using BugReporter.Models.Events;
using BugReporter.Models.Models;
using System;
using System.Collections.Generic;

namespace BugReporter.Bll.Client
{
    public class BugReporterClientCtrl : IBugReporterObserver
    {
        private readonly IBugReporterServer _server;
        private User _currentUser;
        public event EventHandler<BugReporterEventArgs> UpdateEvent;

        public BugReporterClientCtrl(IBugReporterServer server)
        {
            this._server = server;
            _currentUser = null;
        }

        public User Login(String username, String password)
        {
            User user = new User()
            {
                Username = username,
                Password = password
            };

            var result = _server.Login(user, this);
            _currentUser = result;

            return result;
        }
        
        public void Logout()
        {
            _server.Logout(_currentUser, this);
            _currentUser = null;
        }

        public void GetReports()
        {
            _server.GetReports(this);
        }

        public UserList GetUserList()
        {
            return _server.GetUserList();
        }

        protected virtual void OnUserEvent(BugReporterEventArgs e)
        {
            if (UpdateEvent == null) return;
            
            UpdateEvent(this, e);
        }

        public void SaveChanges(ReportModel reportModel)
        {
            _server.SaveReports(reportModel);
        }

        public void UpdateReports(ReportModel reportModel)
        {
            BugReporterEventArgs userArgs = new BugReporterEventArgs(BugReporterUserEvents.UpdateBugList, reportModel);
            OnUserEvent(userArgs);
        }
    }
}
