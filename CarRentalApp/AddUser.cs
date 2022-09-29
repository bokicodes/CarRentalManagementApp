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
    public partial class AddUser : Form
    {
        private readonly CarRentalEntities _db;
        private ManageUsers _manageUsers;
        public AddUser(ManageUsers manageUsers)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _manageUsers = manageUsers;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = _db.Roles.ToList();
            cbRoles.DataSource = roles;
            cbRoles.DisplayMember = "name";
            cbRoles.ValueMember = "id";

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var username = tbUserName.Text;
                if (string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("You must enter username!");
                    return;
                }
                var roleId = (int)cbRoles.SelectedValue;
                var password = Utils.GenericHashedPassword();

                var user = new User
                {
                    username = username,
                    password = password,
                    isActive = true
                };
                _db.Users.Add(user);
                _db.SaveChanges();

                var userId = user.id;
                var userRole = new UserRole
                {
                    roleid = roleId,
                    userid = userId
                };
                _db.UserRoles.Add(userRole);
                _db.SaveChanges();

                MessageBox.Show("New User Added Successfully",
                    "User Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _manageUsers.fillGrid();
                Close();
                

                
            }
            catch (Exception)
            {
                MessageBox.Show("An Error Has Occured");
            }
        }
    }
}
