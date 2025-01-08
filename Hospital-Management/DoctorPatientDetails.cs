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
    public partial class DoctorPatientDetails : Form
    {
        public int doctorId = 0;
        public int patientId = 0;

        public DoctorPatientDetails(int doctorId, int patientId)
        {
            InitializeComponent();

            UserData userData = new UserData();
            this.doctorId = userData.GetDoctorIdByUserId(doctorId);
            this.patientId = patientId;

            dataGridView1.DataSource = GetPatientDiagnosis(patientId);
            dataGridView2.DataSource = GetPatientTreatment(patientId);

            PatientData patientData = new PatientData();
            profilePictureBox.Image = patientData.GetProfileImage(patientId);

            loadData(patientId);
        }

        private void addRecordButton_Click(object sender, EventArgs e)
        {
            MedicalRecordsData medicalRecordsData = new MedicalRecordsData();
            medicalRecordsData.InsertMedicalRecord(patientId, doctorId, diagnosisTextBox.Text, treatmentTextBox.Text);

            MessageBox.Show("Record added");
        }

        public void homeButton_Click(object sender, EventArgs e)
        {
            DoctorDashboard d = new DoctorDashboard(doctorId);
            d.Show();
            this.Hide();
        }

        public void patientList_Click(object sender, EventArgs e)
        {
            DoctorPatientList pl = new DoctorPatientList(doctorId);
            pl.Show();
            this.Hide();
        }

        public void appointments_Click(object sender, EventArgs e)
        {
            DoctorAppointments ap = new DoctorAppointments(doctorId);
            ap.Show();
            this.Hide();
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
                        
                        patientNameTextBox.Text = reader["first_name"].ToString() + " " + reader["last_name"].ToString();

                        
                        DateTime dob = Convert.ToDateTime(reader["dob"]);

                        genderTextBox.Text = reader["gender"].ToString();
                        
                        int age = CalculateAge(dob);
                        ageTextBox.Text = age.ToString();
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

        private int CalculateAge(DateTime dob)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dob.Year;
            
            if (dob > today.AddYears(-age)) age--;

            return age;
        }

        public DataTable GetPatientDiagnosis(int patientId)
        {
            DataTable diagnosisTable = new DataTable();

            string connectionString = "Server = localhost\\SQLEXPRESS; Database = hospitalDatabase; Integrated Security = True; ";
            string query = "SELECT diagnosis FROM MedicalRecords WHERE patient_id = @PatientId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PatientId", patientId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(diagnosisTable);
                    }
                }
            }

            return diagnosisTable;
        }

        public DataTable GetPatientTreatment(int patientId)
        {
            DataTable treatmentTable = new DataTable();

            string connectionString = "Server = localhost\\SQLEXPRESS; Database = hospitalDatabase; Integrated Security = True; ";
            string query = "SELECT treatment FROM MedicalRecords WHERE patient_id = @PatientId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PatientId", patientId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(treatmentTable);
                    }
                }
            }

            return treatmentTable;
        }

    }
}
