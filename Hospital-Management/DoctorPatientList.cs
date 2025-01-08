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
    public partial class DoctorPatientList : Form
    {
        public int doctorId = 0;

        public DoctorPatientList(int id)
        {
            InitializeComponent();

            doctorId = id;

            loadData();
        }

        private void loadData()
        {
            PatientData patientData = new PatientData();
            patientData.LoadPatientsIntoDataGridView(patientData.LoadDatabaseSchemaAndData(), dataGridView1);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {

                int patientId = (int)(dataGridView1.Rows[e.RowIndex].Cells["patient_id"].Value);

                if (senderGrid.Columns[e.ColumnIndex].Name == "Details")
                {
                    DoctorPatientDetails pd = new DoctorPatientDetails(doctorId, patientId);
                    pd.Show();
                    loadData();
                }
            }
        }

        public void homeButton_Click(object sender, EventArgs e)
        {
            DoctorDashboard d = new DoctorDashboard(doctorId);
            d.Show();
            this.Hide();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Details.DisplayIndex = dataGridView1.Columns.Count - 1;
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
