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
    public partial class DoctorPatientDetails : Form
    {
        public DoctorPatientDetails()
        {
            InitializeComponent();
        }

        public void homeButton_Click(object sender, EventArgs e)
        {
            DoctorDashboard d = new DoctorDashboard();
            d.Show();
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
