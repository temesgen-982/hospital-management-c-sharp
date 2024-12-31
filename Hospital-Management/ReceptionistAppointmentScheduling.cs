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
    public partial class ReceptionistAppointmentScheduling : Form
    {

        public int receptionistId = 0;

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
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Select"].Index && e.RowIndex >= 0)
            {
                int patientId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["patient_id"].Value);
                
                DoctorsList DoctorsList = new DoctorsList(patientId);
                DoctorsList.Show();
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
