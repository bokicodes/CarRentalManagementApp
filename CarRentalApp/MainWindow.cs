using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class MainWindow : Form
    {
        private Login _login;
        public string _roleShortName;
        public User _user;
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(Login login, User user)
        {
            InitializeComponent();
            _login = login;
            _user = user;
            _roleShortName = user.UserRoles.FirstOrDefault().Role.shortname;

        }
        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openForm = Application.OpenForms.Cast<Form>();
            Boolean isOpen = openForm.Any(q => q.Name == "AddEditRentalRecord");
            ManageRentalRecords manageRentalRecords = new ManageRentalRecords();
            if (!isOpen)
            {
                AddEditRentalRecord addRentalRecord = new AddEditRentalRecord(manageRentalRecords);
                addRentalRecord.MdiParent = this;
                addRentalRecord.Show();
            }
        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Checking if there is any open form with the name ManageCarListing
            var openForm = Application.OpenForms.Cast<Form>();
            Boolean isOpen = openForm.Any(q => q.Name == "ManageCarListing");

            //If there isn't then you can open one
            if (!isOpen)
            {
                ManageCarListing carListing = new ManageCarListing();
                carListing.MdiParent = this;
                carListing.Show();
            }
        }

        private void viewAllRecordsStripMenuItem_Click(object sender, EventArgs e)
        {
            var openForm = Application.OpenForms.Cast<Form>();
            Boolean isOpen = openForm.Any(q => q.Name == "ManageRentalRecords");
            if (!isOpen)
            {
                ManageRentalRecords manageRentalRecords = new ManageRentalRecords()
                {
                    MdiParent = this
                };
                manageRentalRecords.Show();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Close();
        }

        private void manageUsersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var openForm = Application.OpenForms.Cast<Form>();
            var isOpen = openForm.Any(q => q.Name == "ManageUsers");

            if (!isOpen)
            {
                ManageUsers manageUsers = new ManageUsers();
                manageUsers.MdiParent = this;
                manageUsers.Show();
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            string userName = _user.username;
            loggedInAsText.Text = $"Logged in as: {userName}";

            if (_roleShortName != "admin")
            {
                manageUsersToolStripMenuItem1.Visible = false;
            }

        }
    }
}