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
    public partial class ReceptionistDashboard : Form
    {
        public int receptionistId = 0;

        public ReceptionistDashboard(int id)
        {
            InitializeComponent();

            receptionistId = id;

            UserData userData = new UserData();

            PatientData patientData = new PatientData();

            receptionistName.Text += userData.GetUserFullName(id);

            totalPatientsTextBox.Text = patientData.CountPatientsByReceptionist(userData.GetReceptionistIdByUserId(id)).ToString();

            profilePictureBox.Image = userData.GetProfileImage(id);

            AppointmentsData appointmentsData = new AppointmentsData();

            txtAppointmentsBooked.Text = appointmentsData.CountAppointmentsByReceptionist(userData.GetReceptionistIdByUserId(id)).ToString();
        }

        private void patientRegistrationButton_Click(object sender, EventArgs e)
        {
            PatientInformation pr = new PatientInformation(0, receptionistId);
            pr.Show();
        }
      
        private void appointmentSchedulingButton_Click(object sender, EventArgs e)
        {
                ReceptionistAppointmentScheduling asc = new ReceptionistAppointmentScheduling(receptionistId);
                asc.Show();
                this.Hide();

        }
        private void patientListButton_Click( object sender, EventArgs e)
        {
            ReceptionistPatientList pl = new ReceptionistPatientList(receptionistId);
            pl.Show();
            this.Hide();
        }
        public void logoutButton_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }
    }
}
