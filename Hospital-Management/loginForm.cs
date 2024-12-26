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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {

        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Server=localhost;Database=hospitalDatabase;Integrated Security=True;";

            string query = "SELECT Password, role_id FROM Users WHERE first_name = @username";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["password"].ToString();
                                string role = reader["role_id"].ToString();

                                if (storedPassword == password)
                                {
                                    MessageBox.Show("Login successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    showRolePage(role);
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Invalid password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void signupBtn_Click(object sender, EventArgs e)
        {
            signupForm signup = new signupForm();
            signup.Show();
        }

        private void showRolePage(string role)
        {
            switch (role)
            {
                case "Admin":
                    AdminDashboard ad = new AdminDashboard();
                    ad.Show();
                    break;
                case "Doctor":
                    DoctorDashboard dd = new DoctorDashboard();
                    dd.Show();
                    break;
                case "Nurse":
                    NurseDashboard nd = new NurseDashboard();
                    nd.Show();
                    break;
                case "Receptionist":
                    ReceptionistDashboard rd = new ReceptionistDashboard();
                    rd.Show();
                    break;
                case "Billing Officer":
                    BillingOfficerDashboard bd = new BillingOfficerDashboard();
                    bd.Show();
                    break;
            }
        }

        private void password_CheckedChange(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }
    }
}
