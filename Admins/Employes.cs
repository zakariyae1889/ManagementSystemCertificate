using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using ManagmentSystemCertificate.Models;

namespace ManagmentSystemCertificate.Admins
{
    public partial class Employes : Form
    {
        public Employes()
        {
            InitializeComponent();
        }
        private  bool ValidateFormFildes(Control.ControlCollection controls)
        {
            foreach(Control control in controls)
            {
                if (control.Name == "guna2TextBox8") { continue; }
                if(control is Guna.UI2.WinForms.Guna2TextBox txtbox)
                {
                    if (string.IsNullOrWhiteSpace(txtbox.Text))
                    {
                        MessageBox.Show("الرجاء ملء الحقل");
                        txtbox.Focus();
                        return false;
                    }
                }
                if(control is Guna.UI2.WinForms.Guna2ComboBox combox)
                {
                    if (combox.SelectedIndex==-1)
                    {
                        MessageBox.Show("الرجاء اختيار قيمة مت القائمة");
                        combox.Focus();
                        return false;
                    }
                    
                }
                if (control.HasChildren)
                {
                    if (!ValidateFormFildes(control.Controls))
                    {
                        return false;
                    }
                }

            }
            return true;
        }
        private void loadData()
        {
            using(var db=new ManagementCertificateEntities())
            {
                try
                {
                    var employes = db.Employes.ToList();
                    DataGridEmploye.AutoGenerateColumns = false;
                    DataGridEmploye.DataSource = employes;
                }
                catch(Exception ex)
                {

                }
            }
        }



        private void Employes_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch
            {

            }
        }
    }
}
