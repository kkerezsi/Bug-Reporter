using BugReporter.Bll.Client;
using BugReporter.Models.Models;
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
    public partial class DeveloperScreen : Form
    {
        private Form _login { get; set; }
        private BugReporterClientCtrl _clientController;
        private User _user;

        public DeveloperScreen(Form login, BugReporterClientCtrl clientController, User user)
        {
            InitializeComponent();

            _login = login;
            _clientController = clientController;
            _user = user;
            InitLogin();
        }

        private void InitLogin()
        {
            this.Username.Text = _user.Username;
        }

        private void DeveloperScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            _clientController.Logout();
            _login.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _clientController.Logout();
            _login.Show();
            this.Close();
        }
    }
}
