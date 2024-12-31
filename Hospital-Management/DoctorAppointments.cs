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

            doctorId = id;
        }

        public void homeButton_Click(object sender, EventArgs e)
        {
            DoctorDashboard d = new DoctorDashboard(doctorId);
            d.Show();
            this.Hide();
        }

        public void patientDetails_Click(object sender, EventArgs e)
        {
            DoctorPatientDetails pd = new DoctorPatientDetails(doctorId);
            pd.Show();
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
