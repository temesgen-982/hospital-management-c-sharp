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
    public partial class BillingOfficerDashboard : Form
    {
        public BillingOfficerDashboard()
        {
            InitializeComponent();

            Billing billing = new Billing();

            List<Billing> listData = billing.getBillings();

            dataGridView1.DataSource = listData;
        }
    }
}
