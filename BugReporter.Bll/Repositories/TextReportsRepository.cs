using BugReporter.Bll.Repositories.Contracts;
using BugReporter.Models.Enums;
using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            ReportModel report_1 = ReadFromFile();

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

            SaveReportModel();
        }

        public ReportModel ReadFromFile()
        {
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader("reports.txt");

            var reportModel = new ReportModel();

            while ((line = file.ReadLine()) != null)
            {
                var nrOfProjects = Convert.ToInt32(line);

                for (int i = 0; i < nrOfProjects; i++)
                {
                    var projectLine = file.ReadLine();

                    var projectObj = new Project();

                    projectObj.Name = projectLine.Trim();

                    var nrOfBugs = Convert.ToInt32(file.ReadLine());

                    for (int j = 0; j < nrOfBugs; j++)
                    {
                        var rowToTokens = file.ReadLine().Split(',');
                        projectObj.Bugs.Add(new Bug()
                        {
                            Name = rowToTokens[0],
                            AssignedTo = rowToTokens[1],
                            Description = rowToTokens[2],
                            Status = rowToTokens[3]
                        });
                    }

                    reportModel.Projects.Add(projectObj);
                }
            }

            file.Close();

            return reportModel;
        }

        public void SaveReportModel()
        {
            try
            {
                var finalString = "";

                var report = _reports[0];

                finalString += report.Projects.Count + "\r\n";

                foreach (var project in report.Projects)
                {
                    finalString += project.Name + "\r\n";

                    finalString += project.Bugs.Count + "\r\n";

                    foreach (var bug in project.Bugs)
                    {
                        finalString += bug.Name + "," + bug.AssignedTo + "," + bug.Description + "," + bug.Status + "\r\n";
                    }
                }

                File.WriteAllText("reports.txt", finalString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save file");
            }
        }
    }
}
