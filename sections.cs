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
    public partial class sections : Form
    {


        public sections()
        {
            InitializeComponent();
            Con = new Functions();
            ShowSection();

        }

        Functions Con;

        private void ShowSection()
        {
            string Query = "SELECT * FROM SectionTbl";
            SectionList.DataSource = Con.GetData(Query);
            SectionList.Refresh(); // Refresh the DataGridView if needed
        }




        



        private void add_Click(object sender, EventArgs e)
        {
            
        }
        private void SectionList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        private void delete_Click(object sender, EventArgs e)
        {
            
        }


        int Key = 0;
        private void edit_Click(object sender, EventArgs e)
        {
            try
            {
                string Name = SNameTb.Text;
                string Cap = CapacityTb.Text;
                string Desc = DescTb.Text;

                // Adjust the query to avoid specifying the identity column
                string Query = "Update SectionTbl set SName='{0}', Capacity={1}, SDescription='{2}' where SNo={3}";
                Query = string.Format(Query, Name, Cap, Desc, Key);

                // Execute the update command
                Con.SetData(Query);
                MessageBox.Show("Section Updated!!!");
                ShowSection();
            }
            catch (Exception ex)
            {
                // Show the error message
                MessageBox.Show("An error occurred while updating the section: " + ex.Message);
            }

        }

        private void sections_Load(object sender, EventArgs e)
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
            this.Hide();
            Form nextForm = new main();
            nextForm.ShowDialog();
            this.Close();

        }

        private void SectionList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid row is clicked
            if (e.RowIndex >= 0)
            {
                // Automatically select the entire row
                SectionList.CurrentRow.Selected = true;

                // Populate text boxes with the selected row's data
                SNameTb.Text = SectionList.CurrentRow.Cells[1].Value.ToString();
                CapacityTb.Text = SectionList.CurrentRow.Cells[2].Value.ToString();
                DescTb.Text = SectionList.CurrentRow.Cells[3].Value.ToString();
                Key = Convert.ToInt32(SectionList.CurrentRow.Cells[0].Value);
            }
            else
            {
                Key = 0; // No valid selection
            }

        }

        private void add_Click_1(object sender, EventArgs e)
        {
            if (SNameTb.Text == "" || CapacityTb.Text == "" || DescTb.Text == "")
            {
                MessageBox.Show("Missing data!!!");
            }
            else
            {
                try
                {
                    string Name = SNameTb.Text;
                    string Cap = CapacityTb.Text;
                    string Desc = DescTb.Text;

                    string Query = "INSERT INTO SectionTbl VALUES ('{0}', {1}, '{2}')";
                    Query = string.Format(Query, Name, Cap, Desc);


                    // Execute the insert command
                    Con.SetData(Query);
                    MessageBox.Show("Section Added!!!");
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
                string Name = SNameTb.Text;
                string Cap = CapacityTb.Text;
                string Desc = DescTb.Text;

                // Adjust the query to avoid specifying the identity column
                string Query = "Update SectionTbl set SName='{0}', Capacity={1}, SDescription='{2}' where SNo={3}";
                Query = string.Format(Query, Name, Cap, Desc, Key);

                // Execute the update command
                Con.SetData(Query);
                MessageBox.Show("Section Updated!!!");
                ShowSection();
            }
            catch (Exception ex)
            {
                // Show the error message
                MessageBox.Show("An error occurred while updating the section: " + ex.Message);
            }
        }

        private void delete_Click_1(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a section to delete.");
                return;
            }

            // Ask for confirmation before deletion
            var confirmResult = MessageBox.Show("Are you sure you want to delete this section?",
                                                 "Confirm Delete",
                                                 MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.No)
            {
                // User chose not to delete
                return;
            }

            try
            {
                // Adjust the query to avoid specifying the identity column
                string Query = "DELETE FROM SectionTbl WHERE SNo = {0}";
                Query = string.Format(Query, Key);

                // Execute the delete command and get affected rows
                int rowsAffected = Con.SetData(Query);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Section Deleted!!!");
                    ShowSection(); // Refresh data
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

        private void close_Click_1(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}


