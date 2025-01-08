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
    public partial class PatientInformation : Form
    {

        public int receptionistId = 0;

        public SqlConnection connection = new SqlConnection("Server = localhost\\SQLEXPRESS; Database = hospitalDatabase; Integrated Security = True;");

        private string selectedImagePath = null;

        public PatientInformation(int patientId, int userId)
        {
            InitializeComponent();

            if (patientId == 0)
            {
                ClearAllTextboxes();

                UserData userData = new UserData();
                receptionistId = userData.GetReceptionistIdByUserId(userId);
            }
            else
            {
                loadData(patientId);
            }
        }

        private void loadData(int patientId)
        {

            try
            {
                connection.Open();
                string query = "SELECT * FROM Patients WHERE patient_id = @patientId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@patientId", patientId);

                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        //patientId.Text = patientId.ToString();
                        firstNameTextBox.Text = reader["first_name"].ToString();
                        firstNameTextBox.Text = reader["last_name"].ToString();
                        emailTextBox.Text = reader["email"].ToString();
                        phoneTextBox.Text = reader["phone"].ToString();
                        dobDateTimePicker.Value = Convert.ToDateTime(reader["dob"]);
                        contactTextBox.Text = reader["emergency_contact"].ToString();
                        addressTextBox.Text = reader["address"].ToString();
                        string gender = reader["gender"].ToString();
                        if (gender == "M")
                        {
                            maleRadioButton.Checked = true;
                            femaleRadioButton.Checked = false;
                        }
                        else if (gender == "F")
                        {
                            femaleRadioButton.Checked = true;
                            maleRadioButton.Checked = false;
                        }
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

        private void addPatientButton_Click(object sender, EventArgs e)
        {
            // Validate all fields are filled
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(lastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(addressTextBox.Text) ||
                string.IsNullOrWhiteSpace(emailTextBox.Text) ||
                string.IsNullOrWhiteSpace(contactTextBox.Text) ||
                string.IsNullOrWhiteSpace(phoneTextBox.Text) ||
                !maleRadioButton.Checked && !femaleRadioButton.Checked ||
                dobDateTimePicker.Value == null)
            {
                MessageBox.Show("All fields are required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string email = emailTextBox.Text;
            string phone = phoneTextBox.Text;
            string contactInfo = contactTextBox.Text;
            string address = addressTextBox.Text;
            char gender = maleRadioButton.Checked ? 'M' : 'F';

            if (selectedImagePath == null)
            {
                MessageBox.Show("Please upload a profile picture!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] profileImage = File.ReadAllBytes(selectedImagePath);

            DateTime dob = dobDateTimePicker.Value;

            try
            {
                PatientData patientData = new PatientData();
                patientData.InsertPatient(firstName, lastName, dob, gender, phone, address, email, contactInfo, receptionistId, profileImage);

                MessageBox.Show("patient registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearAllTextboxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void ClearAllTextboxes()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textbox)
                {
                    textbox.Clear();
                }
            }

            maleRadioButton.Checked = false;
            femaleRadioButton.Checked = false;
            dobDateTimePicker.Value = DateTime.Now;
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
                    
                }
            }
        }
    }
}