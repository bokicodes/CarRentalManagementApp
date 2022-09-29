using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddEditCar : Form
    {
        private Boolean isEditMode;
        private readonly CarRentalEntities _db; // = new CarRentalEntities();
        private ManageCarListing _manageCarListing;
        public AddEditCar(ManageCarListing manageCarListing = null)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _manageCarListing = manageCarListing;
            lbTitle.Text = "Add Car";
            this.Text = "Add Car";
            isEditMode = false;
        }
        public AddEditCar(TypesOfCar carToEdit, ManageCarListing manageCarListing = null)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _manageCarListing = manageCarListing;
            lbTitle.Text = "Edit Car";
            this.Text = "Edit Car";
            isEditMode = true;
            PopulateFields(carToEdit);
        }

        private void PopulateFields(TypesOfCar carToEdit)
        {
            lbId.Text = carToEdit.Id.ToString(); //not visible on form, just to save somewhere Id we want to edit
            tbMake.Text = carToEdit.Make;
            tbModel.Text = carToEdit.Model;
            tbVIN.Text = carToEdit.VIN;
            tbYear.Text = carToEdit.Year.ToString();
            tbLicensePlateNumber.Text = carToEdit.LicensePlateNumber;
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbMake.Text) || string.IsNullOrEmpty(tbModel.Text) || string.IsNullOrEmpty(tbYear.Text))
            {
                MessageBox.Show("Please enter a make, a model and a year!");
            }
            else
            {
                if (isEditMode)
                {
                    // Edit Code Here
                    int id = int.Parse(lbId.Text); //Id we want to edit
                    var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);
                    car.Make = tbMake.Text;
                    car.Model = tbModel.Text;
                    car.VIN = tbVIN.Text;
                    car.LicensePlateNumber = tbLicensePlateNumber.Text;
                    try
                    {
                        car.Year = int.Parse(tbYear.Text);
                        _db.SaveChanges();
                        MessageBox.Show("Car Successfully Edited","Edit Operation Completed",
                            MessageBoxButtons.OK,MessageBoxIcon.Information);
                        _manageCarListing.FillGrid();                      
                        Close();
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Please enter year as a number!");
                    }
                }
                else
                {
                    // Add Code Here
                    TypesOfCar newCar = new TypesOfCar();
                    newCar.Make = tbMake.Text;
                    newCar.Model = tbModel.Text;
                    newCar.VIN = tbVIN.Text;
                    newCar.LicensePlateNumber = tbLicensePlateNumber.Text;
                    try
                    {
                        newCar.Year = int.Parse(tbYear.Text);
                        _db.TypesOfCars.Add(newCar);
                        _db.SaveChanges();
                        MessageBox.Show("Car Successfully Added", "Adding Operation Completed",
                            MessageBoxButtons.OK,MessageBoxIcon.Information);
                        _manageCarListing.FillGrid();
                        Close();
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Please enter year as a number!");
                    }
                }

            }
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
