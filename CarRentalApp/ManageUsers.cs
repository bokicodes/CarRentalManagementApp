using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities _db;
        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnResetPW_Click(object sender, EventArgs e)
        {
            try
            {
                //get the selected id
                int id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;
                // query database for record
                var user = _db.Users.FirstOrDefault(t => t.id == id);

                // ENCRYPTING THE PASSWORD 
                string hashedPassword = Utils.GenericHashedPassword();

                user.password = hashedPassword;
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s password has been reset to a default one",
                    "Password Reset",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("You need to click on arrow to select the whole row!");
            }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            try
            {
                //get the selected id
                int id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;
                // query database for record
                var user = _db.Users.FirstOrDefault(t => t.id == id);

                if(user.isActive == true)
                {
                    user.isActive = false;
                    MessageBox.Show($"User {user.username} is deactivated!",
                    "Deactivation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    user.isActive = true;
                    MessageBox.Show($"User {user.username} is activated!",
                    "Activation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                _db.SaveChanges();

                fillGrid();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("You need to click on arrow to select the whole row!");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var openForms = Application.OpenForms.Cast<Form>();
            var isOpen = openForms.Any(q => q.Name == "AddUser");

            if (!isOpen)
            {
                AddUser addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }
        }

        public void fillGrid()
        {
            var users = _db.Users.Select(q => new
            {
                q.id,
                Username = q.username,
                Role = q.UserRoles.FirstOrDefault().Role.name,
                Active = q.isActive
            }).ToList();

            gvUserList.DataSource = users;
            gvUserList.Columns["id"].Visible = false;
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            fillGrid();
        }
    }
}
