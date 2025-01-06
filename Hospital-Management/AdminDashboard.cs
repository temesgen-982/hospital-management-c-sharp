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
    public partial class AdminDashboard : Form
    {
        public int adminId = 0;

        public AdminDashboard(int id)
        {
            InitializeComponent();

            adminId = id;

            UserData userData = new UserData();

            adminName.Text += userData.GetUserFullName(id);

            LoadCounts();
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

        private void performanceMetrics_Click(object sender, EventArgs e)
        {
            AdminPerformanceMetrics pm = new AdminPerformanceMetrics(adminId);
            pm.Show();
            this.Hide();
        }

        private void LoadCounts()
        {
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;";

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
                                textBox1.Text = reader["TotalUsers"].ToString();
                                textBox2.Text = reader["TotalUsers"].ToString();
                                textBox3.Text = reader["TotalUsers"].ToString();
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
    }
}
