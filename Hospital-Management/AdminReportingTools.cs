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
    public partial class AdminReportingTools : Form
    {
        public int adminId = 0;

        public AdminReportingTools(int id)
        {
            InitializeComponent();

            adminId = id;
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            AdminDashboard ad = new AdminDashboard(adminId);
            ad.Show();
            this.Hide();
        }

        private void userManagement_Click(object sender, EventArgs e)
        {
            AdminUserManagement um = new AdminUserManagement(adminId);
            um.Show();
            this.Hide();
        }

        private void systemAlerts_Click(object sender, EventArgs e)
        {
            AdminSystemAlerts sa = new AdminSystemAlerts(adminId);
            sa.Show();
            this.Hide();
        }

        private void performanceMetrics_Click(object sender, EventArgs e)
        {
            AdminPerformanceMetrics pm = new AdminPerformanceMetrics(adminId);
            pm.Show();
            this.Hide();
        }
    }
}
