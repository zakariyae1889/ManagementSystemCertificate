using ManagmentSystemCertificate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ManagmentSystemCertificate.Employes
{
    public partial class CitizenView : Form
    {
        

        public CitizenView()
        {
            InitializeComponent();

          

        }

        // load data citizens
        private void loadData()
        {
            try
            {
              
                using (var db=new ManagementCertificateEntities())
                { 
                    var citizens = db.Citizen.ToList();
                    DataGridCitizens.AutoGenerateColumns = false;
                    DataGridCitizens.DataSource = citizens;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("فشل في تحميل بيانات المواطنين:\n" + ex.Message);
            }
            
        }
        // check controlers
        private bool ValidateFormFields(Control.ControlCollection controls)
        {
            foreach(Control control in controls)
            {
                if (control.Name == "guna2TextBox8") continue;
                if (control is Guna.UI2.WinForms.Guna2TextBox txtBox)
                {
                  
                    if (string.IsNullOrWhiteSpace(txtBox.Text))
                    {
                        MessageBox.Show($"الرجاء ملء حقل");
                        txtBox.Focus();
                        return false;
                    }
                }
                else if (control is Guna.UI2.WinForms.Guna2ComboBox comBox)
                {
                    if (comBox.SelectedIndex == -1)
                    {
                        MessageBox.Show($"الرجاء اختيار قيمة من القائمة ");
                        comBox.Focus();
                        return false;
                    }

                }
                else if(control.HasChildren)
                {
                    if (!ValidateFormFields(control.Controls))
                    {
                        return false;
                    }
                   

                }
                
            }
            return true;
        }
        private void Citizen_Load(object sender, EventArgs e)
        {
            loadData();   
        }




        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                using(var db=new ManagementCertificateEntities())
                {
                    if (!ValidateFormFields(this.Controls)) return;
                    
                    var citizens = new Citizen();
                    citizens.NationalID = guna2TextBox1.Text.Trim();
                    citizens.FullName = guna2TextBox2.Text.Trim();
                    citizens.City = guna2TextBox3.Text.Trim();
                    citizens.Address = guna2TextBox4.Text.Trim();
                    citizens.DateBrith = guna2DateTimePicker1.Value;
                    citizens.Sex = guna2ComboBox1.SelectedItem.ToString();
                   
                    db.Citizen.Add(citizens);
                    db.SaveChanges();
                    MessageBox.Show("تمت الإضافة بنجاح");
                    loadData();





                    guna2TextBox1.Text = "";
                    guna2TextBox2.Text = "";
                    guna2TextBox3.Text = "";
                    guna2TextBox4.Text = "";
                }
            }catch(Exception ex)
            {
                MessageBox.Show("خطاء في ادخل المعلومات");
            }
        }

        //
       

        private void guna2TextBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                using (var db = new ManagementCertificateEntities())
                {
                    if (string.IsNullOrWhiteSpace(guna2TextBox8.Text))
                    {
                        loadData();

                        return;
                    }
                    var Shearch = db.Citizen.Where(c => c.NationalID.Contains(guna2TextBox8.Text)).ToList();
                    DataGridCitizens.AutoGenerateColumns = false;
                    DataGridCitizens.DataSource = Shearch;

                }

            }
            catch (Exception ex)
            {

            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
           
        }
    }
}
