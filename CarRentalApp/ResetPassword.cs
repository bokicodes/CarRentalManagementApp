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
    public partial class ResetPassword : Form
    {
        private readonly CarRentalEntities _db;
        private User _user;
        public ResetPassword(User user)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _user = user;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                string password = tbPassword.Text;
                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("You must enter a password");
                    return;
                }
                string confirmPassword = tbConfirmPassword.Text;
                if (password != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match, please try again.");
                    return;
                }
                var user = _db.Users.FirstOrDefault(q => q.id == _user.id);
                user.password = Utils.HashPassword(password);
                _db.SaveChanges();
                MessageBox.Show("Password reseted successfully");
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong, try again");
            }
        }
    }
}
