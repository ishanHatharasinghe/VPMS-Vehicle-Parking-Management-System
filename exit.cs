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
    public partial class exit : Form
    {
        public exit()
        {
            InitializeComponent();
            Con = new Functions();
            ShowSection();
            LoadSections();
            UpdateVehicleNumbers(); // Call the method to load vehicle numbers initially
            vno.SelectedIndexChanged += new EventHandler(vno_SelectedIndexChanged);
            this.Load += new EventHandler(exit_Load);
        }

        Functions Con;
        int Key = 0;

        private void exit_Load(object sender, EventArgs e)
        {
            UpdateVehicleNumbers();
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


        private void UpdateVehicleNumbers()
        {
            // Get all vehicle numbers from EntryTbl
            string query = "SELECT VNo FROM EntryTbl";
            DataTable entryDt = Con.GetData(query);
            List<string> availableVNos = new List<string>();

            // Add all vehicle numbers from EntryTbl to the list
            foreach (DataRow row in entryDt.Rows)
            {
                availableVNos.Add(row["VNo"].ToString());
            }

            // Get vehicle numbers from ExitTbl to remove from available list
            string exitQuery = "SELECT VNo FROM ExitTbl";
            DataTable exitDt = Con.GetData(exitQuery);
            foreach (DataRow row in exitDt.Rows)
            {
                string vNo = row["VNo"].ToString();
                if (availableVNos.Contains(vNo))
                {
                    availableVNos.Remove(vNo); // Remove vehicle numbers that are in ExitTbl
                }
            }

            // Bind the remaining vehicle numbers to the ComboBox
            vno.DataSource = availableVNos;
        }

        private void ShowSection()
        {
            string Query = "SELECT * FROM ExitTbl";
            ExitList.DataSource = Con.GetData(Query);
            ExitList.Refresh();
        }

        private void LoadSections()
        {
            string Query1 = "SELECT SName FROM SectionTbl";
            DataTable dt = Con.GetData(Query1);
            sectionname.DataSource = dt;
            sectionname.DisplayMember = "SName";
            sectionname.ValueMember = "SName";
        }

        private void vno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (vno.SelectedItem != null)
            {
                string selectedVehicleNo = vno.SelectedValue.ToString();
                string Query3 = "SELECT PlateNo, SName FROM EntryTbl WHERE VNo = '" + selectedVehicleNo + "'";
                DataTable dt = Con.GetData(Query3);

                if (dt.Rows.Count > 0)
                {
                    textBox1.Text = dt.Rows[0]["PlateNo"].ToString();
                    sectionname.SelectedValue = dt.Rows[0]["SName"].ToString();
                }
            }
        }

        private void add_Click(object sender, EventArgs e)
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
                    DateTime selectedDateTime = dateTimePicker.Value;
                    string formattedExitTime = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                    string Query = "INSERT INTO ExitTbl (SName, VNo, PlateNo, ExitTime) VALUES ('{0}', '{1}', '{2}', '{3}')";
                    Query = string.Format(Query, SectionName, VehicleNumber, PlateNo, formattedExitTime);
                    Con.SetData(Query);
                    MessageBox.Show("Exit Data Added!!!");

                    // Refresh the data display and vehicle numbers
                    ShowSection();
                    UpdateVehicleNumbers(); // Reload vehicle numbers after adding to ExitTbl
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void EntryList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid row is clicked
            if (e.RowIndex >= 0)
            {
                ExitList.CurrentRow.Selected = true;

                // Populate text boxes with the selected row's data
                vno.SelectedItem = ExitList.CurrentRow.Cells[1].Value.ToString();
                sectionname.SelectedItem = ExitList.CurrentRow.Cells[2].Value.ToString();
                textBox1.Text = ExitList.CurrentRow.Cells[3].Value.ToString();

                Key = Convert.ToInt32(ExitList.CurrentRow.Cells[0].Value);
            }
            else
            {
                Key = 0; // No valid selection
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a record to delete.");
                return;
            }

            // Ask user for confirmation
            DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Deletion", MessageBoxButtons.YesNo);

            // If user clicks "Yes", proceed with deletion
            if (result == DialogResult.Yes)
            {
                try
                {
                    string Query = "DELETE FROM ExitTbl WHERE ExitNo = {0}";
                    Query = string.Format(Query, Key);
                    Con.SetData(Query); // Execute the delete query

                    MessageBox.Show("Record Deleted!!!");
                    ShowSection();  // Refresh the section data
                    UpdateVehicleNumbers();  // Reload vehicle numbers after deletion
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Error: " + Ex.Message);
                }
            }
            // If user clicks "No", the code exits the method here, and nothing happens
        }


        private void ExitList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid row is clicked
            if (e.RowIndex >= 0)
            {
                ExitList.CurrentRow.Selected = true;

                // Populate text boxes with the selected row's data
                vno.Text = ExitList.CurrentRow.Cells[1].Value.ToString(); // Set the vehicle number in the ComboBox
                sectionname.Text = ExitList.CurrentRow.Cells[2].Value.ToString(); // Set the section name in the ComboBox
                textBox1.Text = ExitList.CurrentRow.Cells[3].Value.ToString(); // Set the PlateNo in the TextBox

                // Assign the ExitNo (primary key) from the selected row to 'Key'
                Key = Convert.ToInt32(ExitList.CurrentRow.Cells[0].Value);
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
