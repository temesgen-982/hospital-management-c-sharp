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
    public partial class DoctorProfile : Form
    {
        public int doctorId = 0;
        public string connectionString = "Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;";
        public DoctorProfile(int id)
        {
            InitializeComponent();

            this.doctorId = id;

            UserData userData = new UserData();
            profilePictureBox.Image = userData.GetProfileImage(id);

            LoadDoctorProfile();
        }

        private void saveChangesButton_Click(object sender, EventArgs e)
        {
            string specialty = specialityTextBox.Text;
            int yearsOfExperience;
            bool isYearsValid = int.TryParse(yearsTextBox.Text, out yearsOfExperience);
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            if (!isYearsValid)
            {
                MessageBox.Show("Please enter a valid number for years of experience.");
                return;
            }

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Update Doctors Table
                    string updateDoctorsQuery = @"
                UPDATE Doctors 
                SET specialty = @Specialty, years_of_experience = @Years 
                WHERE doctor_id = @DoctorId";

                    using (SqlCommand updateDoctorsCommand = new SqlCommand(updateDoctorsQuery, connection))
                    {
                        updateDoctorsCommand.Parameters.AddWithValue("@Specialty", specialty);
                        updateDoctorsCommand.Parameters.AddWithValue("@Years", yearsOfExperience);
                        updateDoctorsCommand.Parameters.AddWithValue("@DoctorId", doctorId);
                        updateDoctorsCommand.ExecuteNonQuery();
                    }

                    // Update Users Table
                    string updateUsersQuery = @"
                UPDATE Users 
                SET username = @Username, password = @Password 
                WHERE user_id = (SELECT user_id FROM Doctors WHERE doctor_id = @DoctorId)";

                    using (SqlCommand updateUsersCommand = new SqlCommand(updateUsersQuery, connection))
                    {
                        updateUsersCommand.Parameters.AddWithValue("@Username", username);
                        updateUsersCommand.Parameters.AddWithValue("@Password", PasswordHasher.HashPassword(password));
                        updateUsersCommand.Parameters.AddWithValue("@DoctorId", doctorId);
                        updateUsersCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Profile updated successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

            this.Hide();
        }


        private void LoadDoctorProfile()
        {
            
            string query = @"
        SELECT u.first_name, u.last_name, u.dob, u.username, u.password, d.specialty, d.years_of_experience
        FROM Users u
        INNER JOIN Doctors d ON u.user_id = d.user_id
        WHERE d.doctor_id = @DoctorId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorId", doctorId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                firstNameTextBox.Text = reader["first_name"].ToString();
                                lastNameTextBox.Text = reader["last_name"].ToString();

                                DateTime dob = Convert.ToDateTime(reader["dob"]);
                                ageTextBox.Text = (DateTime.Now.Year - dob.Year - (DateTime.Now.Date < dob.AddYears(DateTime.Now.Year - dob.Year) ? 1 : 0)).ToString();
                                
                                specialityTextBox.Text = reader["specialty"].ToString();
                                yearsTextBox.Text = reader["years_of_experience"].ToString();
                                
                                usernameTextBox.Text = reader["username"].ToString();
                                passwordTextBox.Text = reader["password"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No profile data found for the doctor.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the profile: {ex.Message}");
            }
        }


    }
}
