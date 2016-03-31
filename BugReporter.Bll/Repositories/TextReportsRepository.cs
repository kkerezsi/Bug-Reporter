using BugReporter.Bll.Repositories.Contracts;
using BugReporter.Models.Enums;
using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugReporter.Bll.Repositories
{
    public class TextReportsRepository : IReportsRepository
    {
        private List<ReportModel> _reports;

        public TextReportsRepository()
        {
            _reports = new List<ReportModel>();

            Initialize();
        }

        private void Initialize()
        {
            ReportModel report_1 = new ReportModel();
            report_1.Projects.Add(new Project()
            {
                Name = "Project 1",
                Id = 1,
                Bugs = new List<Bug>()
                {
                    new Bug()
                    {
                        Description = "Description 1",
                        Name = "Name 1",
                        Status = BugStatus.ToDo ,
                        Reporter = new User()
                        {
                            Username = "Alex1",
                            Type = UserType.Developer
                        }
                    },
                    new Bug()
                    {
                        Description = "Description 2",
                        Name = "Name 2",
                        Status = BugStatus.ToDo ,
                        Reporter = new User()
                        {
                            Username = "Alex2",
                            Type = UserType.Developer
                        }
                    }
                }
            });

            report_1.Projects.Add(new Project()
            {
                Name = "Project 2",
                Id = 2,
                Bugs = new List<Bug>()
                {
                    new Bug()
                    {
                        Description = "Description 3",
                        Name = "Name 3",
                        Status = BugStatus.ToDo ,
                        Reporter = new User()
                        {
                            Username = "Alex3",
                            Type = UserType.Developer
                        }
                    },
                    new Bug()
                    {
                        Description = "Description 4",
                        Name = "Name 4",
                        Status = BugStatus.ToDo ,
                        Reporter = new User()
                        {
                            Username = "Alex4",
                            Type = UserType.Developer
                        }
                    }
                }
            });


            _reports.Add(report_1);
        }

        public ReportModel GetReports()
        {
            if (_reports.Count == 0)
                return new ReportModel();

            return _reports.FirstOrDefault();
        }

        public void SaveReports(ReportModel reportModel)
        {
            _reports.Clear();
            _reports.Add(reportModel);
        }
    }
}
