using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugReporter.Bll.Client;

namespace BugReporter.Bll.Server.Components
{
    public interface IBugReporterServer
    {
        User Login(User user, IBugReporterObserver client);
        void Logout(User user, IBugReporterObserver client);
        ReportModel GetReports(IBugReporterObserver client);
        UserList GetUserList(int? projectId = null);
        void SaveReports(ReportModel reportModel);
    }
}
