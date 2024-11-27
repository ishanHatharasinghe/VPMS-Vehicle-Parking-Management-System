using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vehicle_Parking_Management_System_Project
{
    public partial class vehicles : Form
    {
        public vehicles()
        {
            InitializeComponent();
            Con = new Functions();
            ShowSection();
        }

        Functions Con;

        private void ShowSection()
        {
            string Query = "SELECT * FROM CarTbl"; // Adjust table name if necessary
            VehicleList.DataSource = Con.GetData(Query);
            VehicleList.Refresh(); // Refresh the DataGridView if needed
        }

        private void entries_Load(object sender, EventArgs e)
        {
            UpdateVcountLabel();

            // Timer for automatic updates
            Timer timer = new Timer();
            timer.Interval = 5000; // Refresh every 5 seconds
            timer.Tick += (s, ev) =>
            {
                UpdateVcountLabel();

            };
            timer.Start();
        }
        private void UpdateVcountLabel()
        {
            try
            {
                int capacity = Con.GetCount("SELECT SUM(Capacity) FROM SectionTbl");
                int entryCount = Con.GetCount("SELECT COUNT(*) FROM EntryTbl");
                int exitCount = Con.GetCount("SELECT COUNT(*) FROM ExitTbl");
                int vehicleCount = entryCount - exitCount;
                int availableSpots = capacity - (entryCount - exitCount);

                

                int filledCapacity = entryCount - exitCount;
                Vprog.Minimum = 0;          // Minimum is always 0
                Vprog.Maximum = capacity;   // Maximum is the total capacity
                Vprog.Value = Math.Max(0, Math.Min(filledCapacity, capacity));

                // Calculate the percentage of filled capacity
                double percentageFilled = (capacity > 0) ? ((double)filledCapacity / capacity) * 100 : 0;
                label10.Text = $" {percentageFilled:F2}%";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating " + ex.Message);
            }
        }

        int Key = 0;

        private void VehicleList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void add_Click(object sender, EventArgs e)
        {
            
        }
        private void delete_Click(object sender, EventArgs e)
        {
            
        }
        private void edit_Click(object sender, EventArgs e)
        {
            
        }



        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            // This method is currently empty. You may want to implement any necessary painting here.
        }

        private void add_Click_1(object sender, EventArgs e)
        {
            if (plateno.Text == "" || vtype.Text == "" || colour.Text == "" || drivername.Text == "" || nic.Text == "" || phone.Text == "")
            {
                MessageBox.Show("Missing data!!!");
            }
            else
            {
                try
                {
                    string PlateNo = plateno.Text;
                    string Vtype = vtype.Text;
                    string Colour = colour.Text;
                    string Name = drivername.Text;
                    string Nic = nic.Text;
                    string Pnum = phone.Text;

                    // Correct the SQL query to properly format string values
                    string Query = "INSERT INTO CarTbl (PlateNo, Vtype, Colour, DriverName, DriverNIC, Phone) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";
                    Query = string.Format(Query, PlateNo, Vtype, Colour, Name, Nic, Pnum);

                    // Execute the insert command
                    Con.SetData(Query);
                    MessageBox.Show("Vehicle Added!!!");
                    ShowSection();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void edit_Click_1(object sender, EventArgs e)
        {
            try
            {
                string PlateNo = plateno.Text;
                string Vtype = vtype.Text;
                string Colour = colour.Text;
                string Name = drivername.Text;
                string Nic = nic.Text;
                string Pnum = phone.Text;

                // Adjust the query to update the selected entry
                string Query = "UPDATE CarTbl SET PlateNo = '{0}', Vtype = '{1}', Colour = '{2}', DriverName = '{3}', DriverNIC = '{4}', Phone = '{5}' WHERE VNo = '{6}'";
                Query = string.Format(Query, PlateNo, Vtype, Colour, Name, Nic, Pnum, Key);

                // Execute the update command
                Con.SetData(Query);
                MessageBox.Show("Vehicle Entry Updated!!!");
                ShowSection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void delete_Click_1(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a section to delete.");
                return;
            }

            // Show confirmation dialog
            var confirmResult = MessageBox.Show("Are you sure you want to delete this vehicle data?",
                                                 "Confirm Delete",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Adjust the query to avoid specifying the identity column
                    string Query = "DELETE FROM CarTbl WHERE VNo = {0}";
                    Query = string.Format(Query, Key);

                    // Execute the delete command and get affected rows
                    int rowsAffected = Con.SetData(Query);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Vehicle Data Deleted!!!");
                        ShowSection(); // Refresh data
                    }
                    else
                    {
                        MessageBox.Show("No vehicle found with the specified ID.");
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Error: " + Ex.Message);
                }
            }
            else
            {
                // User chose not to delete
                MessageBox.Show("Deletion cancelled.");
            }
        }

        private void VehicleList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a row is selected
            {
                VehicleList.CurrentRow.Selected = true;
                plateno.Text = VehicleList.CurrentRow.Cells[1].Value.ToString();  // Plate Number
                vtype.Text = VehicleList.CurrentRow.Cells[2].Value.ToString();    // Vehicle Type
                colour.Text = VehicleList.CurrentRow.Cells[3].Value.ToString();  // Colour
                drivername.Text = VehicleList.CurrentRow.Cells[4].Value.ToString(); // Driver Name
                nic.Text = VehicleList.CurrentRow.Cells[5].Value.ToString();      // Driver NIC
                phone.Text = VehicleList.CurrentRow.Cells[6].Value.ToString();     // Phone

                Key = Convert.ToInt32(VehicleList.CurrentRow.Cells[0].Value); // Assuming the first column is the ID
            }
            else
            {
                Key = 0; // No valid selection
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}




