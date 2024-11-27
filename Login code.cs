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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true; // Allows the form to capture key events
            this.KeyDown += new KeyEventHandler(Form1_KeyDown); // Attach the KeyDown event handler
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // This method is triggered when the user clicks the login button
        private void login_Click(object sender, EventArgs e)
        {
            // Get the values entered in TextBox1 and TextBox2
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Check if both fields are empty
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                label4.Text = "Please enter username";
                label5.Text = "Please enter the password";
            }
            // Check if only the username field is empty
            else if (string.IsNullOrEmpty(username))
            {
                label4.Text = "Please enter username";
            }
            // Check if only the password field is empty
            else if (string.IsNullOrEmpty(password))
            {
                label5.Text = "Please enter the password";
            }
            else
            {
                // Validate credentials
                if (username == "admin" && password == "123")
                {
                    // If valid, close the current form and open the next form
                    this.Hide(); // Hide the login form
                    Form nextForm = new main(); // Replace with your next form class
                    nextForm.ShowDialog(); // Open the next form
                    this.Close(); // Close the login form
                }
                else
                {
                    // Check if both username and password are incorrect
                    if (username != "a" && password != "1")
                    {
                        MessageBox.Show("Invalid username & password, please enter the correct ones.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // Check if only the username is incorrect
                    else if (username != "a")
                    {
                        MessageBox.Show("Invalid username, please enter the correct one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // Check if only the password is incorrect
                    else if (password != "1")
                    {
                        MessageBox.Show("Invalid password, please enter the correct one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        // If valid, close the current form and open the next form
                        this.Hide(); // Hide the login form
                        Form nextForm = new main(); // Replace with your next form class
                        nextForm.ShowDialog(); // Open the next form
                        this.Close(); // Close the login form
                    }
                }
            }
        }

        // This method captures key presses in the form
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // If the Enter key is pressed, trigger the login button click
            if (e.KeyCode == Keys.Enter)
            {
                login.PerformClick(); // Trigger the login button's click event
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // Close the application completely when the close button is clicked
            Application.Exit();
        }
    }
}
