using BugReporter.Bll.Client;
using BugReporter.Models.Enums;
using BugReporter.Models.Events;
using BugReporter.Models.Models;
using BugReporter.Models.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugReporter.Client
{
    public partial class TesterScreen : Form
    {
        private Form _login { get; set; }
        private BugReporterClientCtrl _clientController;
        private User _user;
        private ReportModel _reports;

        public TesterScreen(Form login, BugReporterClientCtrl clientController, User user)
        {
            InitializeComponent();

            _clientController = clientController;
            _login = login;
            _user = user;
            _clientController.UpdateEvent += UserUpdate;
            InitLogin();
        }

        private void InitLogin()
        {
            this.Username.Text = _user.Username;
            _reports = _clientController.GetReports();

            PopulateDropDownProjects(_reports);
            PopulateGridView(_reports);
            _clientController.IsHandleCreated = this.IsHandleCreated;
        }

        private void PopulateDropDownProjects(ReportModel _reports)
        {
            foreach (var item in _reports.Projects)
            {
                this.ProjectList.Items.Add(item.Name);
            }

            if (this.ProjectList.Items.Count > 0)
            {
                this.ProjectList.SelectedIndex = 0;
            }
        }

        private void PopulateGridView(ReportModel reports, string selectedItem = "")
        {
            this.BugReports.Rows.Clear();
            if (string.IsNullOrEmpty(selectedItem))
                selectedItem = this.ProjectList.SelectedItem.ToString();

            var selectedProject = _reports.Projects.FirstOrDefault(x => x.Name.Equals(selectedItem));

            foreach (var item in selectedProject.Bugs)
            {
                this.BugReports.Rows.Add(item.Name, "", item.Description, "");
            }
        }

        private void TesterScreen_Load(object sender, EventArgs e)
        {

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _clientController.Logout();
            _login.Show();
            this.Close();
        }

        private void TesterScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            _clientController.Logout();
            _clientController.UpdateEvent -= UserUpdate;
            _login.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void BugReports_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ProjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BugReports.Rows.Clear();
            var selectedProject = _reports.Projects.FirstOrDefault(x => x.Name.Equals(this.ProjectList.SelectedItem));

            foreach (var item in selectedProject.Bugs)
            {
                this.BugReports.Rows.Add(item.Name, "", item.Description, "");
            }
        }

        public void UserUpdate(object sender, BugReporterEventArgs e)
        {
            if (e.UserEventType == BugReporterUserEvents.UpdateBugList)
            {
                BugReports.BeginInvoke(new UpdateReportListCallback(this.UpdateReportList), new Object[] { BugReports, e.Data });
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //for updating the GUI
        //1. define a method for updating the ReportList
        private void UpdateReportList(DataGridView reportList, ReportModel newData)
        {
            BugReports.Rows.Clear();

            _reports = newData;
            var selected = ProjectList.SelectedItem;

            PopulateDropDownProjects(_reports);
            PopulateGridView(_reports, selected != null ? selected.ToString() : string.Empty);
        }

        //2. define a delegate to be called back by the GUI Thread
        public delegate void UpdateReportListCallback(DataGridView reportList, ReportModel data);

        private void BugReports_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }

        private void BugReports_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void BugReports_RowLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SaveData_Click(object sender, EventArgs e)
        {
            ReportModel report = ProjectConverter.ConvertStringListToProject(ProjectList.SelectedItem.ToString(), BugReports, _reports);
            _clientController.SaveChanges(report);
        }
    }
}
