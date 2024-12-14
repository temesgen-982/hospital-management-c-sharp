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
    public partial class NursePatientList : Form
    {
        public NursePatientList()
        {
            InitializeComponent();

            Patient patient = new Patient();

            List<Patient> patients = patient.getPatients();

            dataGridView1.DataSource = patients;
        }
    }
}
