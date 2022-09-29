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
    public partial class ManageCarListing : Form
    {
        private readonly CarRentalEntities _db;
        public ManageCarListing()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void ManageCarListing_Load(object sender, EventArgs e)
        {
            //Select * from TypesOfCars
            //var cars = _db.TypesOfCars.ToList();

            //Select id as CarId, name as CarName from TypesOfCars
            //var cars = _db.TypesOfCars.
            //Select(q => new { CarId = q.Id, CarName = q.Name }).ToList();
            //I did this because I don't want CarRentalRecord column to show up on my grid
            //so this way I selected only first two columns, id and name

            //I decided to include these columns 
            var cars = _db.TypesOfCars.
                Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicensePlateNumber = q.LicensePlateNumber,
                    Id = q.Id
                }).ToList();
            gvCarList.DataSource = cars; //list of cars from database is datasource for my grid
            //gvCarList.Columns[0].HeaderText = "ID"; //renaming header text of columns
            //gvCarList.Columns[1].HeaderText = "NAME";
            gvCarList.Columns["LicensePlateNumber"].HeaderText = "License Plate Number";
            gvCarList.Columns["Id"].Visible = false; //I don't want Id to appear
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var openForm = Application.OpenForms.Cast<Form>();
            Boolean isOpen = openForm.Any(q => q.Name == "AddEditCar");

            if (!isOpen)
            {
                AddEditCar addCar = new AddEditCar(this);
                addCar.MdiParent = this.MdiParent; //MainWindow is the parent of ManageCarListing
                addCar.Show();
            }           
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // get id of a selected row
            try
            {
                int id = (int)gvCarList.SelectedRows[0].Cells["Id"].Value;
                // query database for record
                TypesOfCar car = _db.TypesOfCars.FirstOrDefault(t => t.Id == id);
                //                                                      where

                var openForm = Application.OpenForms.Cast<Form>();
                Boolean isOpen = openForm.Any(q => q.Name == "AddEditCar");

                if (!isOpen)
                {
                    // launch AddEditCar window with data
                    AddEditCar editCar = new AddEditCar(car,this);

                    //Set parent and show form
                    editCar.MdiParent = this.MdiParent;
                    editCar.Show();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("You need to click on arrow to select the whole row!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // get id of a selected row
                int id = (int)gvCarList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                TypesOfCar car = _db.TypesOfCars.FirstOrDefault(t => t.Id == id);

                DialogResult result = MessageBox.Show("Are you sure you want to delete this car? " +
                    "If you delete this car, all rental records with this car will also be deleted!", "Delete", 
                    MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning);
                if(result == DialogResult.Yes)
                {
                    _db.TypesOfCars.Remove(car);
                    _db.SaveChanges();                   
                }
                //Refresh the grid function
                FillGrid();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("You need to click on arrow to select the whole row!");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FillGrid();
        }
        //New Function to PopulateGrid. Can be called anytime we need a grid refresh
        public void FillGrid()
        {
            // Select a custom model collection of cars from database
            var cars = _db.TypesOfCars
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicensePlateNumber = q.LicensePlateNumber,
                    q.Id
                })
                .ToList();
            gvCarList.DataSource = cars;
            gvCarList.Columns[4].HeaderText = "License Plate Number";
            //Hide the column for ID. Changed from the hard coded column value to the name, 
            // to make it more dynamic. 
            gvCarList.Columns["Id"].Visible = false;
        }
    }
}
