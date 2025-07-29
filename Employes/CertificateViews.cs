
using ManagmentSystemCertificate.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ManagmentSystemCertificate.Employes
{
    public partial class CertificateViews : Form
    {
        public CertificateViews()
        {
            InitializeComponent();
        }
        private void loadData()
        {
            try
            {
                using (var db = new ManagementCertificateEntities())
                {
                    var result = (from certificate in db.Certificate
                                  join request in db.CertificateRequest on certificate.idRequest equals request.id
                                  join citizens in db.Citizen on request.idCitizens equals citizens.id
                                  join cerfType in db.certificateType on request.idCertificateType equals cerfType.id
                                  select new
                                  {
                                      Nationaid = citizens.NationalID,
                                      Name = citizens.FullName,
                                      Statusrequest = request.RequestStatus,
                                      StatusCertifcate = certificate.status,
                                      TypeCertificate = cerfType.NameCertificate,
                                      DateRequest = request.RequestDate,
                                      DateDelivery = certificate.date_delivery,
                                  }).ToList();
                    dataGridCertificate.AutoGenerateColumns = false;
                    dataGridCertificate.DataSource = result;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل البيانات: " + ex.Message);
            }
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string cin = guna2TextBox1.Text.Trim();
                if (string.IsNullOrWhiteSpace(cin))
                {
                    MessageBox.Show("الحقل فارغ.");
                    return;
                }
                using(var db =new ManagementCertificateEntities())
                {
                    var citizens = db.Citizen.FirstOrDefault(c => c.NationalID == cin);

                    if (citizens==null)
                    {
                        MessageBox.Show("المواطن غير موجود.");
                        return;
                    }
                    var lastRequest = db.CertificateRequest.Where(r => r.idCitizens == citizens.id).OrderByDescending(r => r.RequestDate).FirstOrDefault();
                    if (lastRequest == null)
                    {
                        MessageBox.Show("لا يوجد أي طلب شهادة لهذا المواطن.");
                        return;
                    }
                    var certificates = new Certificate
                    {
                        status = "قيد المعالجة",
                        date_delivery = guna2DateTimePicker1.Value,
                        idRequest = lastRequest.id,
                    };
                    db.Certificate.Add(certificates);
                    db.SaveChanges();
                    MessageBox.Show("تمت إضافة الشهادة بنجاح.");
                }
            }
            catch
            {

            }
        }

        private void CertificateViews_Load(object sender, EventArgs e)
        {
            loadData();
        }

       

        private void guna2TextBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                using (var db = new ManagementCertificateEntities())
                {
                    string cin = guna2TextBox8.Text.ToString();

                    if (string.IsNullOrWhiteSpace(cin))
                    {
                        loadData();
                        return;
                    }

                    var result = (from certificate in db.Certificate
                                  join request in db.CertificateRequest on certificate.idRequest equals request.id
                                  join citizens in db.Citizen on request.idCitizens equals citizens.id
                                  join cerfType in db.certificateType on request.idCertificateType equals cerfType.id
                                  where citizens.NationalID == cin
                                  select new
                                  {
                                      Nationaid = citizens.NationalID,
                                      Name = citizens.FullName,
                                      Statusrequest = request.RequestStatus,
                                      StatusCertifcate = certificate.status,
                                      TypeCertificate = cerfType.NameCertificate,
                                      DateRequest = request.RequestDate,
                                      DateDelivery = certificate.date_delivery,
                                  }).ToList();
                    dataGridCertificate.AutoGenerateColumns = false;
                    dataGridCertificate.DataSource = result;
                }
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}
