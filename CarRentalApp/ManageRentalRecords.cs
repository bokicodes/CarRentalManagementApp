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
    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities _db;
        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var openForm = Application.OpenForms.Cast<Form>();
            Boolean isOpen = openForm.Any(q => q.Name == "AddEditRentalRecord");

            if (!isOpen)
            {
                AddEditRentalRecord addRentalRecord = new AddEditRentalRecord(this)
                {
                    MdiParent = this.MdiParent
                };
                addRentalRecord.Show();
            }
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        public void FillGrid()
        {
            // Select a custom model collection of cars from database
            var records = _db.CarRentalRecords
                .Select(q => new
                {
                    q.id,
                    q.CustomerName,
                    q.DateRented,
                    q.DateReturned,
                    q.Cost,
                    Car = q.TypesOfCar.Make + " " + q.TypesOfCar.Model, //inner join
                })
                .ToList();
            gvRentalRecordList.DataSource = records;
            gvRentalRecordList.Columns["id"].Visible = false;
            gvRentalRecordList.Columns["CustomerName"].HeaderText = "Customer Name";
            gvRentalRecordList.Columns["DateRented"].HeaderText = "Date Rented";
            gvRentalRecordList.Columns["DateReturned"].HeaderText = "Date Returned";

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // get id of a selected row
            try
            {
                int selectedId = (int)gvRentalRecordList.SelectedRows[0].Cells["Id"].Value;
                // query database for record
                CarRentalRecord recordToEdit = _db.CarRentalRecords.FirstOrDefault(t => t.id == selectedId);
                //                                                                  where

                var openForm = Application.OpenForms.Cast<Form>();
                Boolean isOpen = openForm.Any(q => q.Name == "AddEditRentalRecord");

                if (!isOpen)
                {
                    // launch Edit Car window with data
                    AddEditRentalRecord editRecord = new AddEditRentalRecord(recordToEdit,this);

                    //Set parent and show form
                    editRecord.MdiParent = this.MdiParent;
                    editRecord.Show();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("You need to click on arrow to select the whole row!");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // get id of a selected row
                int selectedId = (int)gvRentalRecordList.SelectedRows[0].Cells["id"].Value;

                // query database for record
                CarRentalRecord record = _db.CarRentalRecords.FirstOrDefault(t => t.id == selectedId);

                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?",
                    "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    // remove record from table
                    _db.CarRentalRecords.Remove(record);
                    _db.SaveChanges();
                }
                // refresh the grid view
                FillGrid();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("You need to click on arrow to select the whole row!");
            }
        }
    }
}
