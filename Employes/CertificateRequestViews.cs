using ManagmentSystemCertificate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ManagmentSystemCertificate.Employes
{
    public partial class CertificateRequestViews : Form
    {
        public CertificateRequestViews()
        {
            InitializeComponent();
        }

        //Check   Controls 
        private bool ValidateFormFields(Control.ControlCollection controls)
        {
            foreach(Control control in controls)
            {
                if (control.Name == "guna2TextBox8") continue;
                if (control is Guna.UI2.WinForms.Guna2TextBox txtBox)
                {
                    if (string.IsNullOrWhiteSpace(txtBox.Text))
                    {
                        MessageBox.Show($"الرجاء اختيار قيمة من القائمة ");
                        txtBox.Focus();
                        return false;
                    }
                }
                else if (control is Guna.UI2.WinForms.Guna2ComboBox Combox)
                {
                    if (Combox.SelectedIndex == -1)
                    {
                        MessageBox.Show($"الرجاء اختيار قيمة من القائمة ");
                        Combox.Focus();
                        return false;
                    }
                }
                else if (control.HasChildren)
                {
                    if (!ValidateFormFields(control.Controls))
                    {
                        return false;
                    }

                }
            }

            return true;
        }

        // load Data in datagridview
        private void loadData()
        {
            try
            {

                using (var db = new ManagementCertificateEntities())
                {
                    var data = from certificateRequest in db.CertificateRequest
                               join citizens in db.Citizen on certificateRequest.idCitizens equals citizens.id
                               join certificateType in db.certificateType on certificateRequest.idCertificateType equals certificateType.id
                               select new
                               {
                                   Nationalid=citizens.NationalID,
                                   Name=citizens.FullName,
                                   Category=certificateType.certificateCategory.CertificateCategory1,
                                   NameCertificate=certificateType.NameCertificate,
                                   RequestDate=certificateRequest.RequestDate,
                                   RequestStatus=certificateRequest.RequestStatus,
                               };
                                dataGridRequestCertificate.AutoGenerateColumns = false;
                                dataGridRequestCertificate.DataSource = data.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل في تحميل بيانات المواطنين:\n" + ex.Message);
            }



        }
        // load combox category certificate
        private void loadDataCategory()
        {
            try
            {
                using(var db= new ManagementCertificateEntities())
                {
                    var category = db.certificateCategory.Select(cat => new
                    {
                        id = cat.id,
                        categorys = cat.CertificateCategory1
                    }).ToList() ;
                    guna2ComboBox3.DataSource = category;
                    guna2ComboBox3.DisplayMember = "categorys";
                    guna2ComboBox3.ValueMember = "id";
                }
                
            }
            catch
            {

            }
        }
        private void CertificateRequestViews_Load(object sender, EventArgs e)
        {
            loadDataCategory();
            loadData();
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedid = Convert.ToInt32(guna2ComboBox3.SelectedValue);
                using (var db = new ManagementCertificateEntities())
                {
                    var certificatesType = db.certificateType.Where(sub => sub.CategoryId == selectedid).Select(sub => new
                    {

                        id = sub.id,
                        name=sub.NameCertificate,
                    }).ToList();
                    guna2ComboBox1.DataSource = certificatesType;
                    guna2ComboBox1.DisplayMember = "name";
                    guna2ComboBox1.ValueMember = "id";


                }

            }
            catch
            {

            }
        }
        //add information
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string cin = guna2TextBox1.Text.Trim();
                int selectTypeId = Convert.ToInt32(guna2ComboBox1.SelectedValue);

                if (!ValidateFormFields(this.Controls)) { return; }

                using (var db = new ManagementCertificateEntities())
                {
                    var citizens = db.Citizen.FirstOrDefault(c=>c.NationalID==cin);

                    if (citizens == null)
                    {
                        MessageBox.Show("المواطن غير موجو");   
                        return; 
                    }
                    var request = new CertificateRequest
                    {

                        idCitizens = citizens.id,
                        idCertificateType = selectTypeId,
                        RequestDate = guna2DateTimePicker1.Value,
                        RequestStatus = guna2ComboBox2.Text.ToString()
                    };
                    db.CertificateRequest.Add(request);
                    db.SaveChanges();
                    MessageBox.Show("تمت إضافة الطلب بنجاح");
                    guna2TextBox1.Clear();
                    guna2ComboBox2.SelectedIndex = -1;
                    guna2ComboBox1.SelectedIndex = -1;
                    guna2ComboBox3.SelectedIndex = -1;
                    loadData();
                }

            }
            catch 
            {

            }
        }
        //update information
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                string cin = guna2TextBox1.Text.Trim();
                int selectTypeId = Convert.ToInt32(guna2ComboBox1.SelectedValue);
                using (var db=new ManagementCertificateEntities())
                {
                    var requestUpdate = (from Certificaterequest in db.CertificateRequest
                                         join citizens in db.Citizen on Certificaterequest.idCitizens equals citizens.id
                                         join Certificatetype in db.certificateType on Certificaterequest.idCertificateType equals Certificatetype.id
                                         where citizens.NationalID == cin
                                         select Certificaterequest).FirstOrDefault();
                    if (requestUpdate != null)
                    {
                        requestUpdate.RequestStatus = guna2ComboBox2.Text.ToString();
                        requestUpdate.RequestDate = guna2DateTimePicker1.Value;
                        requestUpdate.idCertificateType = selectTypeId;
                        db.SaveChanges();
                        MessageBox.Show("تم تحديث الحالة بنجاح");
                        loadData();
                    }
                    else
                    {
                        MessageBox.Show("لم يتم العثور على الطلب");
                    }
                }
            }
            catch
            {

            }
             



        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            try
            {
                var cin = guna2TextBox1.Text.Trim();

                using (var db = new ManagementCertificateEntities())
                {
                    var requestDelete = (from requestCertificate in db.CertificateRequest
                                         join citizens in db.Citizen on requestCertificate.idCitizens equals citizens.id
                                         where citizens.NationalID == cin select  requestCertificate).FirstOrDefault();
                    if (requestDelete != null)
                    {
                        db.CertificateRequest.Remove(requestDelete);
                        db.SaveChanges();
                        MessageBox.Show("تم حذف الطلب بنجاح");
                        loadData();
                    }
                    else
                    {
                        MessageBox.Show("لم يتم العثور على الطلب المطلوب حذفه");
                    }
                }
            }
            catch
            {

            }
        }

        private void guna2TextBox8_KeyPress(object sender, KeyPressEventArgs e)
        {

            try
            {
                string cin = guna2TextBox8.Text.Trim();

                using (var db = new ManagementCertificateEntities())
                {
                    if (string.IsNullOrWhiteSpace(cin))
                    {
                        loadData();
                        return;
                    }

                    var requestSearch = (from Certificaterequest in db.CertificateRequest
                                         join citizens in db.Citizen on Certificaterequest.idCitizens equals citizens.id
                                         join Certificatetype in db.certificateType on Certificaterequest.idCertificateType equals Certificatetype.id
                                         where citizens.NationalID == cin
                                         select new
                                         {
                                             NationalID = citizens.NationalID,
                                             Name = citizens.FullName,
                                             RequestDate = Certificaterequest.RequestDate,
                                             RequestStatus = Certificaterequest.RequestStatus,
                                             NameCertificate = Certificatetype.NameCertificate,
                                             Category = Certificatetype.certificateCategory.CertificateCategory1
                                         }).ToList();

                    dataGridRequestCertificate.AutoGenerateColumns = false;
                    dataGridRequestCertificate.DataSource = requestSearch.Any() ? requestSearch : null;

                    if (!requestSearch.Any())
                    {
                        loadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء جلب البيانات:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
           
        }
    }
}
