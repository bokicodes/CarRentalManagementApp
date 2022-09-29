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
    public partial class Login : Form
    {
        //adding comment
        private readonly CarRentalEntities _db;
        public Login()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {           
            try
            {
                string username = tbUsername.Text.Trim();
                string pw = tbPassword.Text;

                            // ENCRYPTING THE PASSWORD
                string hashedPW = Utils.HashPassword(pw);

                var user = _db.Users.FirstOrDefault(q => q.username == username && q.password == hashedPW
                && q.isActive == true); 

                if(user == null)
                {
                    MessageBox.Show("Something went wrong, please check your username, password" +
                        " and make sure your account is activated!","Error",MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    MainWindow mainWindow = new MainWindow(this, user);
                    mainWindow.Show();
                    this.Hide();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong, please try again.");
            }
        }
    }
}
