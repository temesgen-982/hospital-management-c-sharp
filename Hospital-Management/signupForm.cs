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
        public SignupForm()
        {
            InitializeComponent();
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

            try
            {
                // Insert the user into the Users table
                string connectionString = "Server=localhost\\SQLEXPRESS;Database=hospitalDatabase;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert into Users table
                    string query = @"
                INSERT INTO Users (first_name, last_name, username, dob, email, phone, password, role)
                VALUES (@FirstName, @LastName, @Username, @DateOfBirth, @Email, @Phone, @Password, @Role);
                SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstNameTextBox.Text.Trim());
                        command.Parameters.AddWithValue("@LastName", lastNameTextBox.Text.Trim());
                        command.Parameters.AddWithValue("@Username", usernameTextBox.Text.Trim());
                        command.Parameters.AddWithValue("@DateOfBirth", dobDateTimePicker.Value);
                        command.Parameters.AddWithValue("@Email", emailTextBox.Text.Trim());
                        command.Parameters.AddWithValue("@Phone", phoneTextBox.Text.Trim());
                        command.Parameters.AddWithValue("@Password", passwordTextBox.Text); // Hash password in production
                        command.Parameters.AddWithValue("@Role", roleComboBox.SelectedItem.ToString());

                        int userId = Convert.ToInt32(command.ExecuteScalar());

                        // Assign the user to the respective role-specific table
                        string role = roleComboBox.SelectedItem.ToString();
                        AssignToRole(connection, userId, role);

                        MessageBox.Show("User registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Assign the user to the respective role-specific table
        private void AssignToRole(SqlConnection connection, int userId, string role)
        {
            string roleTable = string.Empty;

            switch (role)
            {
                case "Doctor":
                    roleTable = "Doctors";
                    break;
                case "Nurse":
                    roleTable = "Nurses";
                    break;
                case "Receptionist":
                    roleTable = "Receptionists";
                    break;
                case "Billing Officer":
                    roleTable = "BillingOfficers";
                    break;
                case "Admin":
                    roleTable = "Admins";
                    break;
                default:
                    throw new ArgumentException("Invalid role selected");
            }

            string roleQuery = $@"
        INSERT INTO {roleTable} (user_id, first_name, last_name, dob, email, phone, username)
        VALUES (@UserId, @FirstName, @LastName, @DateOfBirth, @Email, @Phone, @Username);";

            using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
            {
                roleCommand.Parameters.AddWithValue("@UserId", userId);
                roleCommand.Parameters.AddWithValue("@FirstName", firstNameTextBox.Text.Trim());
                roleCommand.Parameters.AddWithValue("@LastName", lastNameTextBox.Text.Trim());
                roleCommand.Parameters.AddWithValue("@DateOfBirth", dobDateTimePicker.Value);
                roleCommand.Parameters.AddWithValue("@Email", emailTextBox.Text.Trim());
                roleCommand.Parameters.AddWithValue("@Phone", phoneTextBox.Text.Trim());
                roleCommand.Parameters.AddWithValue("@Username", usernameTextBox.Text.Trim());
                roleCommand.ExecuteNonQuery();
            }
        }



    }
}