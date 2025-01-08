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
    public partial class DoctorDashboard : Form
    {
        public int doctorId = 0;

        public DoctorDashboard(int id)
        {
            InitializeComponent();

            doctorId = id;

            UserData userData = new UserData();

            doctorName.Text += userData.GetUserFullName(id);

            profilePictureBox.Image = userData.GetProfileImage(id);

            AppointmentsData appointmentsData = new AppointmentsData();
            DataTable allAppointments = appointmentsData.GetAppointments(userData.GetDoctorIdByUserId(doctorId));
            if (allAppointments.Rows.Count == 0)
            {
                MessageBox.Show("No appointments found.");
            }
            dataGridView1.DataSource = allAppointments;
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

        public void editProfile_Click(object sender, EventArgs e)
        {
            UserData userData = new UserData();

            DoctorProfile dp = new DoctorProfile(userData.GetDoctorIdByUserId(doctorId));
            dp.Show();
        }

        public void logoutButton_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }
    }
}
