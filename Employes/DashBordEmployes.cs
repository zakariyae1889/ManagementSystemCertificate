using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagmentSystemCertificate.Employes
{
    public partial class DashBordEmployes : Form
    {
        public DashBordEmployes()
        {
            InitializeComponent();
        }
        private Form activeForm;

        private void openChailedForm(Form chailedForm)
        {
            if(chailedForm!=null && chailedForm == activeForm) { return; }
            activeForm = chailedForm;
            chailedForm.TopLevel = false;
            chailedForm.FormBorderStyle = FormBorderStyle.None;
            chailedForm.Dock = DockStyle.Fill;
            this.panelDesktopPane.Controls.Add(chailedForm);
            chailedForm.BringToFront();
            chailedForm.Show();

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            openChailedForm(new Employes.CitizenView());
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            openChailedForm(new Employes.CertificateRequestViews());
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            openChailedForm(new Employes.CertificateViews());
        }

        private void DashBordEmployes_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                ExportExcel excel = new ExportExcel();
                excel.ExportAllDataInOneFile();
                MessageBox.Show("تم ارسال المعلومات بنجاح");
            }
            catch(Exception ex)
            {
                MessageBox.Show("حدث خطاء اثناء الارسال");
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
