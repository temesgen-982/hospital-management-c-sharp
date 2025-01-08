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
    public partial class AdminPerformanceMetrics : Form
    {
        public int adminId = 0;

        public string connectionString = "Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;";

        public AdminPerformanceMetrics(int id)
        {
            InitializeComponent();

            adminId = id;

            LoadCounts();
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            AdminDashboard ad = new AdminDashboard(adminId);
            ad.Show();
            this.Hide();
        }

        private void userManagement_Click(object sender, EventArgs e)
        {
            AdminUserManagement um = new AdminUserManagement(adminId);
            um.Show();
            this.Hide();
        }

        private void systemAlerts_Click(object sender, EventArgs e)
        {
            AdminSystemAlerts sa = new AdminSystemAlerts(adminId);
            sa.Show();
            this.Hide();
        }

        private void LoadCounts()
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to count records
                    string query = @"
                    SELECT
                        (SELECT COUNT(*) FROM Patients) AS TotalPatients,
                        (SELECT COUNT(*) FROM Doctors) AS TotalDoctors,
                        (SELECT COUNT(*) FROM Users) AS TotalUsers,
                        (SELECT COUNT(*) FROM Appointments) AS TotalAppointments;
                ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Assign values to textboxes
                                patientsTextBox.Text = reader["TotalPatients"].ToString();
                                doctorsTextBox.Text = reader["TotalDoctors"].ToString();
                                usersTextBox.Text = reader["TotalUsers"].ToString();
                                appointmentsTextBox.Text = reader["TotalAppointments"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }
        public void logoutButton_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }
    }
}