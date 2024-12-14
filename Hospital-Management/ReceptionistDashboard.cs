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

        private void ReceptionistDashboard_Load(object sender, EventArgs e)
        {

        }

        private void patientRegistration_click(object sender, EventArgs e)
        {
            PatientRegistration pr = new PatientRegistration();
            pr.Show();
            this.Hide();
        }
      
        private void AppointmentScheduling_click(object sender, EventArgs e)
        {
                ReceptionistAppointmentScheduling asc = new ReceptionistAppointmentScheduling();
                asc.Show();
                this.Hide();

        }
        private void ReceptionistPatientList( object sender, EventArgs e)
        {
            ReceptionistPatientList pl = new ReceptionistPatientList();
            pl.Show();
            this.Hide();
        }
    }
}
