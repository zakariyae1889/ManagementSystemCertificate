using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagmentSystemCertificate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            
            
        }
           PasswordChange passwordForm = new PasswordChange();

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.PasswordChar = '*';
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            passwordForm.Show();
            
        }
    }
}
