﻿using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Bll.Server.Components
{
    public interface IBugReporterObserver
    {
        void UpdateReports(ReportModel reportModel);
    }
}
