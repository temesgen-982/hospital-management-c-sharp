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
        public int nurseId = 0;

        public NursePatientList(int id)
        {
            InitializeComponent();

            nurseId = id;

            loadData();
        }

        private void loadData()
        {
            PatientData patientData = new PatientData();
            patientData.LoadPatientsIntoDataGridView(patientData.LoadDatabaseSchemaAndData(), dataGridView1);
        }
    }
}
