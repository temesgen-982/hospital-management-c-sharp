using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital_Management
{
    public partial class DoctorAppointments : Form
    {
        public int doctorId = 0;

        public DoctorAppointments(int id)
        {
            InitializeComponent();

            UserData userData = new UserData();
            doctorId = userData.GetDoctorIdByUserId(id);

            AppointmentsData appointmentsData = new AppointmentsData();
            DataTable todaysAppointments = appointmentsData.GetAppointments(this.doctorId, DateTime.Today);
            if (todaysAppointments.Rows.Count == 0)
            {
                MessageBox.Show("No appointments found for today.");
            }
            dataGridView1.DataSource = todaysAppointments;

            DataTable allAppointments = appointmentsData.GetAppointments(this.doctorId);
            if (allAppointments.Rows.Count == 0)
            {
                MessageBox.Show("No appointments found.");
            }
            dataGridView2.DataSource = allAppointments;
        }

        public void filterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedOption = filterComboBox.SelectedItem.ToString();

            DataTable resultTable;

            AppointmentsData appointmentsData = new AppointmentsData();
            try
            {
                if (selectedOption == "Patient ID")
                {
                    resultTable = appointmentsData.GetAppointmentsGroupedByPatient(doctorId);
                }
                else if (selectedOption == "Appointment date")
                {
                    resultTable = appointmentsData.GetAppointmentsGroupedByDate(doctorId);
                }
                else
                {
                    MessageBox.Show("Invalid selection.");
                    return;
                }

                dataGridView2.DataSource = resultTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
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
    }
}
