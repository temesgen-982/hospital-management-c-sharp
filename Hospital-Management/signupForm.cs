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

using System.IO;

namespace Hospital_Management
{
    public partial class SignupForm : Form
    {

        private string selectedImagePath = null;

        public SignupForm(string txt)
        {
            InitializeComponent();

            titleLable.Text = txt;
            signupButton.Text = txt;
        }

        private void signupButton_Click(object sender, EventArgs e)
        {
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

            if (selectedImagePath == null)
            {
                MessageBox.Show("Please upload a profile picture!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] profileImage = File.ReadAllBytes(selectedImagePath);

            try
            {
                UserData userData = new UserData();
                userData.InsertUser(firstName, lastName, username, email, phone, dob, password, role, profileImage);

                MessageBox.Show("User registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear fields after successful registration
                //ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void uploadImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Select a Profile Picture";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = openFileDialog.FileName;
                    
                    profilePictureBox.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void backToLoginButton_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void password_CheckedChange(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                passwordTextBox.PasswordChar = '\0';
            }
            else
            {
                passwordTextBox.PasswordChar = '*';
            }
        }

        private void confirmPassword_CheckedChange(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                confirmPasswordTextBox.PasswordChar = '\0';
            }
            else
            {
                confirmPasswordTextBox.PasswordChar = '*';
            }
        }
    }
}