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
    public partial class EditUserForm : Form
    {
        public EditUserForm(int userId)
        {
            InitializeComponent();

            loadData(userId);
        }

        private void loadData(int userId)
        {
            SqlConnection connection = new SqlConnection("Server = localhost\\SQLEXPRESS; Database = hospitalDatabase; Integrated Security = True;");

            try
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        userID.Text = userId.ToString();
                        firstNameTextBox.Text = reader["first_name"].ToString();
                        lastNameTextBox.Text = reader["last_name"].ToString();
                        emailTextBox.Text = reader["email"].ToString();
                        phoneTextBox.Text = reader["phone"].ToString();
                        roleComboBox.Text = reader["role"].ToString();
                        dobDateTimePicker.Value = Convert.ToDateTime(reader["dob"]);
                        passwordTextBox.Text = reader["password"].ToString();
                    }

                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            // Gather data from form controls
            int userId = int.Parse(userID.Text); // Assuming userIDLabel contains only the userId
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string email = emailTextBox.Text;
            string phone = phoneTextBox.Text;
            string role = roleComboBox.Text;
            DateTime dob = dobDateTimePicker.Value;
            string password = passwordTextBox.Text; // Consider hashing the password before storing it

            // Validate input (basic example)
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // Update the user in the database
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string updateQuery = "UPDATE Users SET first_name = @firstName, last_name = @lastName, email = @email, " +
                                         "phone = @phone, role = @role, dob = @dob, password = @password WHERE user_id = @userId";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@lastName", lastName);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.Parameters.AddWithValue("@dob", dob);
                        cmd.Parameters.AddWithValue("@password", password); // Consider hashing

                        cmd.Parameters.AddWithValue("@userId", userId);

                        // Execute the update command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User updated successfully.");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Update failed.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during the update: {ex.Message}");
                }
            }
        }
    }
}
