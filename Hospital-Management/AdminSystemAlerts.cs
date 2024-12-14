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
    public partial class AdminSystemAlerts : Form
    {
        public AdminSystemAlerts()
        {
            InitializeComponent();
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            AdminDashboard ad = new AdminDashboard();
            ad.Show();
            this.Hide();
        }

        private void userManagement_Click(object sender, EventArgs e)
        {
            AdminUserManagement um = new AdminUserManagement();
            um.Show();
            this.Hide();
        }

        private void reportingTools_Click(object sender, EventArgs e)
        {
            AdminReportingTools rt = new AdminReportingTools();
            rt.Show();
            this.Hide();
        }

        private void performanceMetrics_Click(object sender, EventArgs e)
        {
            AdminPerformanceMetrics pm = new AdminPerformanceMetrics();
            pm.Show();
            this.Hide();
        }
    }
}
