using BugReporter.Bll.Client;
using BugReporter.Models.Enums;
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
    public partial class Login : Form
    {
        BugReporterClientCtrl _clientController;

        public Login(BugReporterClientCtrl clientConroller)
        {
            _clientController = clientConroller;
            InitializeComponent();
        }

        public Login()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            String user = Username.Text;
            String pass = Password.Text;
            try
            {
                var loginResult = _clientController.Login(user, pass);

                switch (loginResult.Type)
                {
                    case (UserType.Developer): 
                        { 
                            DevScreen DevScreen = new DevScreen(this, _clientController, loginResult);
                            DevScreen.Show();
                            break;
                        }
                    case (UserType.Tester):
                        {
                            TesterScreen tester = new TesterScreen(this, _clientController, loginResult);
                            tester.Show();
                            break;
                        }
                }

                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Login Error " + ex.Message/*+ex.StackTrace*/, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
