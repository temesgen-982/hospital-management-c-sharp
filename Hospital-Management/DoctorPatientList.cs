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

        public DoctorPatientList()
        {
            InitializeComponent();

            Patient patient = new Patient();

            List<Patient> patients = patient.getPatients();

            dataGridView1.DataSource = patients;
        }

        public void homeButton_Click(object sender, EventArgs e)
        {
            DoctorDashboard d = new DoctorDashboard();
            d.Show();
            this.Hide();
        }

        public void patientDetails_Click(object sender, EventArgs e)
        {
            DoctorPatientDetails pd = new DoctorPatientDetails();
            pd.Show();
            this.Hide();
        }
    }
}
