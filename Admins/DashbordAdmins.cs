using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagmentSystemCertificate
{
    public partial class DashbordAdmins : Form
    {


        //Constructor
       
        public DashbordAdmins()
        {
            InitializeComponent();
           
        }
        private Form activeForm;
        private void OpenChiledForm(Form chiledForm)
        {
            if(activeForm!=null && activeForm == chiledForm) { return; }
            activeForm = chiledForm;
            chiledForm.TopLevel = false;
            chiledForm.FormBorderStyle = FormBorderStyle.None;
            chiledForm.Dock = DockStyle.Fill;
            this.panelDesktopPane.Controls.Add(chiledForm);
            chiledForm.BringToFront();
            chiledForm.Show();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            OpenChiledForm(new Admins.Employes());
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            OpenChiledForm(new Admins.Citizen());
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            OpenChiledForm(new Admins.CertificateViews());
        }

        private void DashbordAdmins_Load(object sender, EventArgs e)
        {

        }
    }
}
