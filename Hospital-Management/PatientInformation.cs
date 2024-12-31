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
    public partial class PatientInformation : Form
    {

        public int receptionistId = 0;

        public PatientInformation(int patientId, int userId)
        {
            InitializeComponent();

            if (patientId == 0)
            {
                ClearAllTextboxes();

                receptionistId = userId;
            }
            else
            {
                loadData(patientId);
            }
        }

        private void loadData(int patientId)
        {
            SqlConnection connection = new SqlConnection("Server = localhost\\SQLEXPRESS; Database = hospitalDatabase; Integrated Security = True;");

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

            // Now `selectedGender` holds the selected gender or remains empty if none is selected.

            DateTime dob = dobDateTimePicker.Value;


            PatientData patientData = new PatientData();
            patientData.InsertPatient(firstName, lastName, dob, gender, phone, address, email, contactInfo, receptionistId);

            MessageBox.Show("patient registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearAllTextboxes();
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
    }
}