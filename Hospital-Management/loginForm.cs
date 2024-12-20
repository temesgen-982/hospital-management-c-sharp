﻿using System;
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
            string role;
            bool found = false;

            List<User> users = User.users;

            for (int i = 0; i < 4; i++)
            {
                if (users[i].Username == username && users[i].Password == password)
                {
                    found = true;
                    switch (users[i].Role)
                    {
                        case "Administrator":
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
