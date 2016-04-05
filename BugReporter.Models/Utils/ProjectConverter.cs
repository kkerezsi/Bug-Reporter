using BugReporter.Models.Enums;
using BugReporter.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugReporter.Models.Utils
{
    public static class ProjectConverter
    {
        public static List<List<string>> ConvertProjectToStringList(ReportModel model)
        {
            return new List<List<string>>();
        }

        public static List<List<string>> ConvertStringListToProject(List<List<string>> model)
        {
            return new List<List<string>>();
        }

        public static ReportModel ConvertStringListToProject(string projectName, DataGridView bugReports, ReportModel currentReports, bool isDeveloper = false)
        {
            var currentProject = currentReports.Projects.Where(x => x.Name.Equals(projectName)).FirstOrDefault();

            var rows = new List<Bug>();

            for (int i = 0; i < bugReports.RowCount - 1 + (isDeveloper ? 1 : 0); i++)
            {
                Bug model = new Bug();

                foreach (DataGridViewCell item in bugReports.Rows[i].Cells)
                {
                    switch (item.ColumnIndex)
                    {
                        case 0:
                            model.Name = item.Value.ToString();
                            break;
                        case 1:
                            model.AssignedTo = item.Value.ToString();
                            break;
                        case 2:
                            model.Description = item.Value.ToString();
                            break;
                        case 3:
                            model.Status = item.Value.ToString();
                            break;
                        default:
                            break;
                    }
                }

                rows.Add(model);
            }


            currentProject.Bugs.Clear();
            currentProject.Bugs.AddRange(rows);

            return currentReports;
        }
    }
}
