using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Bll.Repositories.Contracts
{
    public interface IReportsRepository
    {
        ReportModel GetReports();
        void SaveReports(ReportModel reportModel);
    }
}
