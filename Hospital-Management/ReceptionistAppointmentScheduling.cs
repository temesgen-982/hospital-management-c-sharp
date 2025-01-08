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
    public partial class ReceptionistAppointmentScheduling : Form
    {

        public int receptionistId = 0;
        public int doctorId = 0;
        public int patientId = 0;

        public ReceptionistAppointmentScheduling(int id)
        {
            
            InitializeComponent();

            receptionistId = id;

            loadData();
        }

        public void loadData()
        {
            PatientData patientData = new PatientData();
            patientData.LoadPatientsIntoDataGridView(patientData.LoadDatabaseSchemaAndData(), dataGridView1);
            dataGridView2.DataSource = GetAllDoctors();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Select"].Index && e.RowIndex >= 0)
            {
                this.patientId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["patient_id"].Value);

                MessageBox.Show("Patient Selected");

            }
        }

        public DataTable GetAllDoctors()
        {
            DataTable doctorDetails = new DataTable();

            SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;");

            try
            {
                connection.Open();

                string query = @"
                    SELECT 
                        d.doctor_id, 
                        u.first_name, 
                        u.last_name, 
                        u.email, 
                        u.phone, 
                        u.dob, 
                        d.specialty, 
                        d.years_of_experience
                    FROM Doctors d
                    INNER JOIN Users u ON d.user_id = u.user_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(doctorDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving doctor details: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return doctorDetails;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["SelectDoctor"].Index && e.RowIndex >= 0)
            {
                this.doctorId = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["doctor_id"].Value);

                MessageBox.Show("Doctor Selected");
            }
        }

        private void btnScheduleAppointment_Click(object sender, EventArgs e)
        {
            DateTime appointmentDate = dateTimePicker1.Value;
            
            if (this.doctorId == 0)
            {
                MessageBox.Show("Please select a doctor.");
                return;
            }
            
            using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;"))
            {
                try
                {
                    connection.Open();
                    string query = @"
                INSERT INTO Appointments (patient_id, doctor_id, appointment_date, status)
                VALUES (@patientId, @doctorId, @appointmentDate, 'Scheduled')";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@patientId", this.patientId);
                        command.Parameters.AddWithValue("@doctorId", this.doctorId);
                        command.Parameters.AddWithValue("@appointmentDate", appointmentDate);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Appointment scheduled successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to schedule the appointment.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void patientRegistrationButton_click(object sender, EventArgs e)
        {
            PatientInformation pr = new PatientInformation(0, receptionistId);
            pr.Show();
            this.Hide();
        }

        private void patientListButton_Click(object sender, EventArgs e)
        {
            ReceptionistPatientList pl = new ReceptionistPatientList(receptionistId);
            pl.Show();
            this.Hide();
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            ReceptionistDashboard rd = new ReceptionistDashboard(receptionistId);
            rd.Show();
            this.Hide();
        }
    }
}
