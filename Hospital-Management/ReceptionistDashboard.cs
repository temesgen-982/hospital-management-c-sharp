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
        public ReceptionistDashboard()
        {
            InitializeComponent();
            
        }

        private void patientRegistrationButton_Click(object sender, EventArgs e)
        {
            PatientInformation pr = new PatientInformation(0);
            pr.Show();
            this.Hide();
        }
      
        private void appointmentSchedulingButton_Click(object sender, EventArgs e)
        {
                ReceptionistAppointmentScheduling asc = new ReceptionistAppointmentScheduling();
                asc.Show();
                this.Hide();

        }
        private void patientListButton_Click( object sender, EventArgs e)
        {
            ReceptionistPatientList pl = new ReceptionistPatientList();
            pl.Show();
            this.Hide();
        }
    }
}
