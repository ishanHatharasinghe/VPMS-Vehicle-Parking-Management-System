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
using Vehicle_Parking_Management_System_Project; 

namespace Vehicle_Parking_Management_System_Project
{
    public partial class SearchData : Form
    {
        private Functions functions;


        public SearchData()
        {
            InitializeComponent();
            functions = new Functions();

        }

        private void SearchData_Load(object sender, EventArgs e)
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
                int capacity = functions.GetCount("SELECT SUM(Capacity) FROM SectionTbl");
                int entryCount = functions.GetCount("SELECT COUNT(*) FROM EntryTbl");
                int exitCount = functions.GetCount("SELECT COUNT(*) FROM ExitTbl");
                int vehicleCount = entryCount - exitCount;
                int availableSpots = capacity - (entryCount - exitCount);

                Vcount.Text = vehicleCount.ToString();
                Available.Text = availableSpots.ToString();

                int filledCapacity = entryCount - exitCount;
                Vprog.Minimum = 0;          // Minimum is always 0
                Vprog.Maximum = capacity;   // Maximum is the total capacity
                Vprog.Value = Math.Max(0, Math.Min(filledCapacity, capacity));
                
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

       

        private void Search_Click(object sender, EventArgs e)
        {
            string plateNo = textBox1.Text.Trim();

            // Check if the PlateNo is entered
            if (string.IsNullOrEmpty(plateNo))
            {
                MessageBox.Show("No Plate Number entered. Please enter a valid Plate Number.");
                textBox1.Text = "Enter The Plate Number"; // Set default text
                return;
            }

            // Fetch vehicle data from the database
            DataTable data = functions.GetVehicleData(plateNo);

            // Check if any data is returned
            if (data.Rows.Count == 0)
            {
                MessageBox.Show("No data found for the entered Plate Number.");
                search1.Text = "No data available";
                search2.Text = "No data available";
                search3.Text = "No data available";
                search4.Text = "No Entry";
                search5.Text = "No Entry";
                search6.Text = "No Entry";
                return;
            }

            // Display data in labels (assuming columns from the query in GetVehicleData)
            search1.Text = data.Rows[0]["DriverName"].ToString();
            search2.Text = data.Rows[0]["DriverNIC"].ToString();
            search3.Text = data.Rows[0]["Phone"].ToString();

            // Display entry data if available, otherwise show "No Entry"
            if (data.Rows[0]["SName"] != DBNull.Value)
            {
                search4.Text = data.Rows[0]["SName"].ToString();
                search5.Text = data.Rows[0]["EntryTime"].ToString();
            }
            else
            {
                search4.Text = "No Entry";
                search5.Text = "No Entry";
            }

            // Display exit data if available, otherwise show "No Entry"
            if (data.Rows[0]["ExitTime"] != DBNull.Value)
            {
                search6.Text = data.Rows[0]["ExitTime"].ToString();
            }
            else
            {
                search6.Text = "No Exit Time";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Logout_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

    }
}
