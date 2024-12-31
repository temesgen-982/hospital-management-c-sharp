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
    public partial class NurseDashboard : Form
    {
        public int nurseId = 0;

        public NurseDashboard(int id)
        {
            InitializeComponent();

            nurseId = id;
        }

        private void patientListButton_Click(object sender, EventArgs e)
        {
            NursePatientList patientList = new NursePatientList(nurseId);
            patientList.Show();
        }
    }
}
