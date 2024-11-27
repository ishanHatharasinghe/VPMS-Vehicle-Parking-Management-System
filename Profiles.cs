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
    public partial class Profiles : Form
    {
        public Profiles()
        {
            InitializeComponent();
            Con = new Functions();
            ShowSection();
        }
        Functions Con;

        private void ShowSection()
        {
            string Query = "SELECT * FROM ProfileTbl";
            ProfileList.DataSource = Con.GetData(Query);
            ProfileList.Refresh(); // Refresh the DataGridView if needed
        }



        private void add_Click(object sender, EventArgs e)
        {
            if (name.Text == "" ||
                address.Text == "" ||
                nic.Text == "" ||
                phone.Text == "" ||
                email.Text == "" ||
                gender.Text == "" ||
                post.Text == "" ||
                dep.Text == "" ||
                plate.Text == "" ||
                vtype.Text == "")
            {
                MessageBox.Show("Missing data or no selection!!!");
            }
            else
            {
                try
                {
                    string Name = name.Text;
                    string Address = address.Text;
                    string NIC = nic.Text;
                    string Phone = phone.Text;
                    string Email = email.Text;
                    string Gender = gender.Text;
                    string Post = post.Text;
                    string Department = dep.Text;
                    string PlateNo = plate.Text;
                    string Vtype = vtype.Text;

                    string Query = "INSERT INTO ProfileTbl (FullName,Address,NIC,Phone,Email,Gender,Post,Department,PlateNo,Vtype) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')";
                    Query = string.Format(Query, Name, Address, NIC, Phone, Email, Gender, Post, Department, PlateNo, Vtype);

                    // Execute the insert command
                    Con.SetData(Query);
                    MessageBox.Show("Profile Added!!!");
                    ShowSection();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }




        int Key = 0;

        private void ProfileList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ProfileList.CurrentRow.Selected = true;
                name.Text = ProfileList.CurrentRow.Cells[1].Value.ToString();
                address.Text = ProfileList.CurrentRow.Cells[2].Value.ToString();
                nic.Text = ProfileList.CurrentRow.Cells[3].Value.ToString();
                phone.Text = ProfileList.CurrentRow.Cells[4].Value.ToString();
                email.Text = ProfileList.CurrentRow.Cells[5].Value.ToString();
                gender.Text = ProfileList.CurrentRow.Cells[6].Value.ToString();
                post.Text = ProfileList.CurrentRow.Cells[7].Value.ToString();
                dep.Text = ProfileList.CurrentRow.Cells[8].Value.ToString();
                plate.Text = ProfileList.CurrentRow.Cells[9].Value.ToString();
                vtype.Text = ProfileList.CurrentRow.Cells[10].Value.ToString();

                Key = Convert.ToInt32(ProfileList.CurrentRow.Cells[0].Value); 
            
            }
            else
            {
                Key = 0; // No valid selection
            }
        }




        private void edit_Click(object sender, EventArgs e)
        {
            if (name.Text == "" ||
                address.Text == "" ||
                nic.Text == "" ||
                phone.Text == "" ||
                email.Text == "" ||
                gender.Text == "" ||
                post.Text == "" ||
                dep.Text == "" ||
                plate.Text == "" ||
                vtype.Text == "")
            {
                MessageBox.Show("Missing data or no selection!!!");
            }
            else
            {
                try
                {
                    string Name = name.Text;
                    string Address = address.Text;
                    string NIC = nic.Text;
                    string Phone = phone.Text;
                    string Email = email.Text;
                    string Gender = gender.Text; // Correct mapping
                    string Post = post.Text; // Correct mapping
                    string Department = dep.Text;
                    string PlateNo = plate.Text; // Correct mapping
                    string Vtype = vtype.Text; // Correct mapping

                    string Query = "UPDATE ProfileTbl SET FullName = '{0}', Address = '{1}', NIC='{2}', Phone = '{3}', Email = '{4}', Gender = '{5}', Post = '{6}', Department = '{7}', PlateNo = '{8}', Vtype = '{9}' WHERE ProfileNo = {10}";
                    Query = string.Format(Query, Name, Address, NIC, Phone, Email, Gender, Post, Department, PlateNo, Vtype, Key);

                    // Execute the update command
                    Con.SetData(Query);
                    MessageBox.Show("Profile Updated!!!");
                    ShowSection();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }





        private void delete_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("No profile selected!");
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this profile?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string Query = "DELETE FROM ProfileTbl WHERE ProfileNo = {0}";
                        Query = string.Format(Query, Key);

                        // Execute the delete command
                        Con.SetData(Query);
                        MessageBox.Show("Profile Deleted!!!");
                        ShowSection();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Delete operation canceled.");
                }
            }
        }

        private void Profiles_Load(object sender, EventArgs e)
        {

        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
