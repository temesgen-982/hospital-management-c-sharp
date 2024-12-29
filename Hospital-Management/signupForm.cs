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

namespace Hospital_Management
{
    public partial class SignupForm : Form
    {

        public SignupForm(string txt)
        {
            InitializeComponent();

            titleLable.Text = txt;
            signupButton.Text = txt;
        }

        private void signupButton_Click(object sender, EventArgs e)
        {
            // Validate all fields are filled
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(lastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(emailTextBox.Text) ||
                string.IsNullOrWhiteSpace(phoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(usernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(passwordTextBox.Text) ||
                string.IsNullOrWhiteSpace(confirmPasswordTextBox.Text) ||
                roleComboBox.SelectedItem == null ||
                dobDateTimePicker.Value == null)
            {
                MessageBox.Show("All fields are required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate if passwords match
            if (passwordTextBox.Text != confirmPasswordTextBox.Text)
            {
                MessageBox.Show("Passwords do not match!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string email = emailTextBox.Text;
            string phone = phoneTextBox.Text;
            string role = roleComboBox.Text;
            string username = usernameTextBox.Text;
            DateTime dob = dobDateTimePicker.Value;
            string password = passwordTextBox.Text;

            UserData userData = new UserData();
            userData.InsertUser(firstName, lastName, username, email, phone, dob, password, role);

            MessageBox.Show("User registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}