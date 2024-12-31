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
        public ReceptionistAppointmentScheduling()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void patientRegistrationButton_click(object sender, EventArgs e)
        {
            PatientInformation pr = new PatientInformation(0);
            pr.Show();
            this.Hide();
        }

        private void patientListButton_Click(object sender, EventArgs e)
        {
            ReceptionistPatientList pl = new ReceptionistPatientList();
            pl.Show();
            this.Hide();
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            ReceptionistDashboard rd = new ReceptionistDashboard();
            rd.Show();
            this.Hide();
        }
    }
}
