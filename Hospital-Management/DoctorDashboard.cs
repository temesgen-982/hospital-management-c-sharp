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
        public DoctorDashboard()
        {
            InitializeComponent();
        }

        public void patientDetails_Click(object sender, EventArgs e)
        {
            DoctorPatientDetails pd = new DoctorPatientDetails();
            pd.Show();
            this.Hide();
        }

        public void patientList_Click(object sender, EventArgs e)
        {
            DoctorPatientList pl = new DoctorPatientList();
            pl.Show();
            this.Hide();
        }

        public void appointments_Click(object sender, EventArgs e)
        {
            DoctorAppointments ap = new DoctorAppointments();
            ap.Show();
            this.Hide();
        }
    }
}
