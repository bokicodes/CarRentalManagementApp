using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddEditRentalRecord : Form
    {
        private Boolean isEditMode;
        private readonly CarRentalEntities _db;
        private ManageRentalRecords _manageRentalRecords;
        public AddEditRentalRecord(ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _manageRentalRecords = manageRentalRecords;
            lbTitle.Text = "Add Rental Record";
            this.Text = "Add Record";
            isEditMode = false;
        }
        public AddEditRentalRecord(CarRentalRecord recordToEdit, ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _manageRentalRecords = manageRentalRecords;
            lbTitle.Text = "Edit Rental Record";
            this.Text = "Edit Record";
            isEditMode = true;
            PopulateFields(recordToEdit);
        }
        private void PopulateFields(CarRentalRecord recordToEdit)
        {
            tbCustomerName.Text = recordToEdit.CustomerName;
            tbCost.Text = recordToEdit.Cost.ToString();
            dateRented.Value = (DateTime)recordToEdit.DateRented;
            dateReturned.Value = (DateTime)recordToEdit.DateReturned;
            lbRecordId.Text = recordToEdit.id.ToString();

        }

        private void button1_Click(object sender, EventArgs e) //button1 -> SUBMIT
        {
            try
            {
                string name = tbCustomerName.Text;
                DateTime date1 = dateRented.Value;
                DateTime date2 = dateReturned.Value;
                string type = comboBox.Text;
                double cost = Convert.ToDouble(tbCost.Text);
                string errorMessage = "";
                Boolean isValid = true;

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(type))
                {
                    isValid = false;
                    errorMessage += "Error: Please enter missing data.\n";
                }
                if (date1 > date2)
                {
                    isValid = false;
                    errorMessage += "Error: Illegal date selection.\n";
                }
                if (isValid)
                {
                    if (isEditMode)
                    {
                                // Edit Code Here
                        var id = int.Parse(lbRecordId.Text);
                        CarRentalRecord rentalRecord = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);
                        rentalRecord.CustomerName = name;
                        rentalRecord.DateRented = date1;
                        rentalRecord.DateReturned = date2;
                        rentalRecord.Cost = (decimal)cost;
                        rentalRecord.TypeOfCarId = (int)comboBox.SelectedValue;

                        _db.SaveChanges();
                        //Displaying message on the screen
                        string message = $" Rental Record Edited\n\n" +
                        $"Customer Name: {name}\n" +
                        $"Date Rented: {date1}\n" +
                        $"Date Returned: {date2}\n" +
                        $"Type of Car: {type} \n" +
                        $"Cost: {cost}$\n\n";

                        MessageBox.Show(message,"Edit Operation Completed",
                            MessageBoxButtons.OK,MessageBoxIcon.Information);
                        _manageRentalRecords.FillGrid();
                        Close();
                    }
                    else
                    {
                                // Add Code Here

                        //Storing values into a database
                        CarRentalRecord rentalRecord = new CarRentalRecord();
                        rentalRecord.CustomerName = name;
                        rentalRecord.DateRented = date1;
                        rentalRecord.DateReturned = date2;
                        rentalRecord.Cost = (decimal)cost;
                        rentalRecord.TypeOfCarId = (int)comboBox.SelectedValue;

                        _db.CarRentalRecords.Add(rentalRecord);
                        _db.SaveChanges();

                        //Displaying message on the screen
                        string message = $" New Rental Record Added\n\n" +
                        $"Customer Name: {name}\n" +
                        $"Date Rented: {date1}\n" +
                        $"Date Returned: {date2}\n" +
                        $"Type of Car: {type} \n" +
                        $"Cost: {cost}$\n\n";

                        MessageBox.Show(message, "Adding Operation Completed",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _manageRentalRecords.FillGrid();                       
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch(FormatException)
            {
                MessageBox.Show($"Cost must be entered as a number!");
            }
        }

        private void Form1_Load(object sender, EventArgs e) //Form1 -> AddRentalRecord.cs
        {
            var cars = _db.TypesOfCars.
                Select
                (q => new { Id = q.Id, Name = q.Make + " " + q.Model }).ToList();
            comboBox.DataSource = cars;
            comboBox.DisplayMember = "Name";
            comboBox.ValueMember = "Id";
        }
    }
}
