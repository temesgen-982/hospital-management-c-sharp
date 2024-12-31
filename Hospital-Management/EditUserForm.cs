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
                        userNameTextBox.Text = reader["username"].ToString();
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
            string username = userNameTextBox.Text;
            DateTime dob = dobDateTimePicker.Value;
            string password = passwordTextBox.Text; // Consider hashing the password before storing it

            UserData userData = new UserData();
            userData.UpdateUser(userId, firstName, lastName, username, email, phone, dob, password, role);

            MessageBox.Show("Success");

            this.Hide();
            
        }
    }
}
