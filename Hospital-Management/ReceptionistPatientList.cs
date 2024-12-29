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
    public partial class ReceptionistPatientList : Form
    {
        public ReceptionistPatientList()
        {
            InitializeComponent();

            loadData();
        }

        private void loadData()
        {
            PatientData patientData = new PatientData();
            patientData.LoadPatientsIntoDataGridView(patientData.LoadDatabaseSchemaAndData(), dataGridView1);
        }
    }
}
