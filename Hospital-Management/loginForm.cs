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
    public partial class loginForm : Form
    {
        public loginForm()
        {
            InitializeComponent();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            bool found = false;

            User user = new User();

            List<User> users = user.getUsers();

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Username == username && users[i].Password == password)
                {
                    found = true;
                    switch (users[i].Role)
                    {
                        case "Admin":
                            AdminDashboard admin = new AdminDashboard();
                            admin.Show();
                            break;
                        case "Nurse":
                            NurseDashboard nurse = new NurseDashboard();
                            nurse.Show();
                            break;
                        case "Doctor":
                            DoctorDashboard doctor = new DoctorDashboard();
                            doctor.Show();
                            break;
                        case "Receptionist":
                            ReceptionistDashboard reception = new ReceptionistDashboard();
                            reception.Show();
                            break;
                        case "Billing Officer":
                            BillingOfficerDashboard officer = new BillingOfficerDashboard();
                            officer.Show();
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!found)
            {
                MessageBox.Show("Error", "Invalid username or password.", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
        }

        private void signupBtn_Click(object sender, EventArgs e)
        {
            signupForm signup = new signupForm();
            signup.Show();
        }
    }
}
