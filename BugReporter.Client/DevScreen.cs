using BugReporter.Bll.Client;
using BugReporter.Models.Enums;
using BugReporter.Models.Events;
using BugReporter.Models.Models;
using BugReporter.Models.Networking.Utils;
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
    public partial class DevScreen : Form
    {
        private Form _login { get; set; }
        private BugReporterClientCtrl _clientController;
        private User _user;
        private ReportModel _reports;
        private UserList _userList;
        private KeyValuePair<string, string>[] _listOfStringUsers;
        private KeyValuePair<string, string>[] _listOfStatus;

        public DevScreen(Form login, BugReporterClientCtrl clientController, User user)
        {
            InitializeComponent();

            _clientController = clientController;
            _login = login;
            _user = user;
            _userList = new UserList();
        }

        private void InitLogin()
        {
            this.Username.Text = _user.Username;
            _clientController.GetReports();
            _userList = _clientController.GetUserList();

            _listOfStringUsers = _userList.Users.Select(x => UserConverter.ConvertUserToKeyValuePair(x)).ToArray();
            _listOfStatus = Enum.GetNames(typeof(BugStatus)).ToList().Select(x => new KeyValuePair<string, string>(x, x)).ToArray();

            BugReports.AutoGenerateColumns = false;

            ((DataGridViewComboBoxColumn)BugReports.Columns[1]).DataSource = _listOfStringUsers;
            ((DataGridViewComboBoxColumn)BugReports.Columns[1]).DisplayMember = "Key";
            ((DataGridViewComboBoxColumn)BugReports.Columns[1]).ValueMember = "Value";

            ((DataGridViewComboBoxColumn)BugReports.Columns[3]).DataSource = _listOfStatus;
            ((DataGridViewComboBoxColumn)BugReports.Columns[3]).DisplayMember = "Key";
            ((DataGridViewComboBoxColumn)BugReports.Columns[3]).ValueMember = "Value";
        }

        private void DevScreen_Load(object sender, EventArgs e)
        {
            _clientController.UpdateEvent += UserUpdate;
            BugReports.AllowUserToAddRows = false;

            for (int i = 0; i < BugReports.Columns.Count - 1; i++)
            {
                DataGridViewColumn col = BugReports.Columns[i];

                col.ReadOnly = true;
            }

            InitLogin();
        }

        private void PopulateDropDownProjects(ReportModel _reports, bool changeSelection = false)
        {
            int selectedIndex = 0;

            if (changeSelection && ProjectList.SelectedIndex > 0)
                selectedIndex = ProjectList.SelectedIndex;

            ProjectList.Items.Clear();

            foreach (var item in _reports.Projects)
            {
                this.ProjectList.Items.Add(item.Name);
            }

            if (this.ProjectList.Items.Count > 0)
            {
                this.ProjectList.SelectedIndex = selectedIndex;
            }
        }

        private void PopulateGridView(ReportModel reports, string selectedItem = "")
        {
            this.BugReports.Rows.Clear();
            if (!string.IsNullOrEmpty(selectedItem))
                selectedItem = this.ProjectList.SelectedItem.ToString();

            var selectedProject = _reports.Projects.FirstOrDefault(x => x.Name.Equals(selectedItem));

            if (selectedProject == null)
                selectedProject = _reports.Projects.FirstOrDefault();

            foreach (var item in selectedProject.Bugs)
            {
                BugReports.Rows.Add(item.Name, item.AssignedTo, item.Description, item.Status);
                BugReports.Rows[BugReports.Rows.Count - 1].Cells[1].Value = item.AssignedTo;
                BugReports.Rows[BugReports.Rows.Count - 1].Cells[3].Value = item.Status;
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _clientController.Logout();
            _login.Show();
            this.Close();
        }

        private void DevScreen_FormClosed(object sender, FormClosedEventArgs e)
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
                this.BugReports.Rows.Add(item.Name, item.AssignedTo, item.Description, item.Status);
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

            PopulateDropDownProjects(_reports, true);
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
            ReportModel report = ProjectConverter.ConvertStringListToProject(ProjectList.SelectedItem.ToString(), BugReports, _reports, true);
            _clientController.SaveChanges(report);
        }
    }
}
