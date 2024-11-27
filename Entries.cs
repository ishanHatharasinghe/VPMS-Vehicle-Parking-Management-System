using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Vehicle_Parking_Management_System_Project
{

    public partial class Entries : Form
    {
        public Entries()
        {
            InitializeComponent();
            Con = new Functions();
            ShowSection();
            LoadSections();
            LoadVehicleNumbers(); // Call the method to load vehicle numbers
            vno.SelectedIndexChanged += new EventHandler(vno_SelectedIndexChanged);
            this.Load += new EventHandler(Form2_Load);
        }
         
        Functions Con;
        
        private void ShowSection()
        {
            string Query = "SELECT * FROM EntryTbl";
            EntryList.DataSource = Con.GetData(Query);
            EntryList.Refresh();
        }
        private void LoadSections()
        {
            string Query1 = "SELECT SName FROM SectionTbl";
            DataTable dt = Con.GetData(Query1);

            // Bind the data to ComboBox1
            sectionname.DataSource = dt;
            sectionname.DisplayMember = "SName";
            sectionname.ValueMember = "SName";  // If needed, this can be changed to a section ID
        }
        private void LoadVehicleNumbers()
        {
            string Query2 = "SELECT FormattedVNo FROM CarTbl";
            DataTable dt1 = Con.GetData(Query2);

            // Bind the data to the ComboBox named 'vno'
            vno.DataSource = dt1;
            vno.DisplayMember = "FormattedVNo";
            vno.ValueMember = "FormattedVNo"; 
        }

        // Event handler for when a vehicle number is selected
        private void vno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (vno.SelectedItem != null)
            {
                string selectedVehicleNo = vno.SelectedValue.ToString();

                // Query to fetch the plate number based on the selected vehicle number
                string Query3 = "SELECT PlateNo FROM CarTbl WHERE FormattedVNo = '" + selectedVehicleNo + "'";
                DataTable dt = Con.GetData(Query3);

                if (dt.Rows.Count > 0)
                {
                    // Assuming PlateNo is in the first column of the result
                    textBox1.Text = dt.Rows[0]["PlateNo"].ToString();
                }
            }
        }


        int Key = 0;


        private void EntryList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid row is clicked
            if (e.RowIndex >= 0)
            {
                // Automatically select the entire row
                EntryList.CurrentRow.Selected = true;

                // Populate text boxes with the selected row's data
                sectionname.SelectedItem = EntryList.CurrentRow.Cells[1].Value.ToString();
                vno.SelectedItem = EntryList.CurrentRow.Cells[2].Value.ToString();
                textBox1.Text = EntryList.CurrentRow.Cells[3].Value.ToString();

                Key = Convert.ToInt32(EntryList.CurrentRow.Cells[0].Value);
            }
            else
            {
                Key = 0; // No valid selection
            }
        }



        private void delete_Click_1(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a section to delete.");
                return;
            }

            // Show a confirmation dialog
            var result = MessageBox.Show("Are you sure you want to delete this entry?",
                                           "Confirm Delete",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);

            // Check the user's response
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Adjust the query to avoid specifying the identity column
                    string Query = "DELETE FROM EntryTbl WHERE EntNo = {0}";
                    Query = string.Format(Query, Key);

                    // Execute the delete command and get affected rows
                    int rowsAffected = Con.SetData(Query);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Entry Deleted!!!");
                        ShowSection(); // Refresh data
                        UpdateVehicleNumbers();
                    }
                    else
                    {
                        MessageBox.Show("No section found with the specified ID.");
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Error: " + Ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Deletion cancelled.");
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            UpdateVehicleNumbers(); // Call this to set up the ComboBox on form load
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

                Vcount.Text = vehicleCount.ToString();
                Available.Text = availableSpots.ToString();

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



        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void UpdateVehicleNumbers()
        {
            string query = "SELECT FormattedVNo FROM CarTbl";
            DataTable dt1 = Con.GetData(query);

            // Create a list of vehicle numbers that are currently in the EntryTbl
            List<string> occupiedVNos = new List<string>();

            // Check EntryTbl for occupied vehicle numbers
            string entryQuery = "SELECT VNo FROM EntryTbl";
            DataTable entryDt = Con.GetData(entryQuery);
            foreach (DataRow row in entryDt.Rows)
            {
                occupiedVNos.Add(row["VNo"].ToString());
            }

            // Check ExitTbl for vehicle numbers that can be shown again
            string exitQuery = "SELECT VNo FROM ExitTbl";
            DataTable exitDt = Con.GetData(exitQuery);
            foreach (DataRow row in exitDt.Rows)
            {
                string vNo = row["VNo"].ToString();
                if (occupiedVNos.Contains(vNo))
                {
                    occupiedVNos.Remove(vNo); // Remove it if it's in the exit table
                }
            }

            // Filter the vehicle numbers based on their occupancy status
            DataTable availableVNos = new DataTable();
            availableVNos.Columns.Add("FormattedVNo", typeof(string));

            foreach (DataRow row in dt1.Rows)
            {
                string formattedVNo = row["FormattedVNo"].ToString();
                if (!occupiedVNos.Contains(formattedVNo)) // Only add non-occupied vehicle numbers
                {
                    availableVNos.Rows.Add(formattedVNo);
                }
            }

            // Bind the filtered data to the ComboBox
            vno.DataSource = availableVNos;
            vno.DisplayMember = "FormattedVNo";
            vno.ValueMember = "FormattedVNo";
        }

        private void add_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || sectionname.SelectedItem == null || vno.SelectedItem == null)
            {
                MessageBox.Show("Missing data!!!");
            }
            else
            {
                try
                {
                    string SectionName = sectionname.SelectedValue.ToString();
                    string VehicleNumber = vno.SelectedValue.ToString();
                    string PlateNo = textBox1.Text;

                    // Get the selected date and time from dateTimePicker
                    DateTime selectedDateTime = dateTimePicker.Value;
                    string formattedEntryTime = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                    string query = "INSERT INTO EntryTbl (SName, VNo, PlateNo, EntryTime) VALUES ('{0}', '{1}', '{2}', '{3}')";
                    query = string.Format(query, SectionName, VehicleNumber, PlateNo, formattedEntryTime);

                    // Execute the insert command
                    Con.SetData(query);
                    MessageBox.Show("Entry Added!!!");

                    // Refresh the data display and update the vehicle numbers
                    ShowSection();
                    UpdateVehicleNumbers(); // Refresh the ComboBox
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void add_Click(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void close_Click_1(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}
