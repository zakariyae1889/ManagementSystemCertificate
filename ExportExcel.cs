using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ManagmentSystemCertificate.Admins;
using ManagmentSystemCertificate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagmentSystemCertificate
{
    internal class ExportExcel
    {

        public void ExportAllDataInOneFile()
        {
            try
            {
                using (var db = new ManagementCertificateEntities())
                using (var workbook = new XLWorkbook())
                {
                    //===================== 1️⃣ تصدير المواطنين =====================
                    var citizens = db.Citizen.ToList();
                    if (citizens.Any())
                    {
                        var wsCitizens = workbook.Worksheets.Add("المواطنين");
                        // الأعمدة
                        var headers1 = new List<string>
                        {
                            "البطاقة الوطنية", "الاسم الكامل", "تاريخ الميلاد", "الجنس", "المدينة", "العنوان"
                        };
                        for (int i = 0; i < headers1.Count; i++)
                        {
                            wsCitizens.Cell(1, i + 1).Value = headers1[i];
                            wsCitizens.Cell(1, i + 1).Style.Font.Bold = true;
                            wsCitizens.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        }
                        int row = 2;
                        foreach (var c in citizens)
                        {
                            wsCitizens.Cell(row, 1).Value = c.NationalID;
                            wsCitizens.Cell(row, 2).Value = c.FullName;
                            wsCitizens.Cell(row, 3).Value = c.DateBrith?.ToString("yyyy-MM-dd");
                            wsCitizens.Cell(row, 4).Value = c.Sex;
                            wsCitizens.Cell(row, 5).Value = c.City;
                            wsCitizens.Cell(row, 6).Value = c.Address;
                            row++;
                        }
                        wsCitizens.Columns().AdjustToContents();
                    }
                    //===================== 2️⃣ تصدير طلبات الشهادات =====================
                    var requests = (from request in db.CertificateRequest
                                    join citizen in db.Citizen on request.idCitizens equals citizen.id
                                    join certType in db.certificateType on request.idCertificateType equals certType.id
                                    select new
                                    {
                                        citizen.NationalID,
                                        citizen.FullName,
                                        certType.certificateCategory.CertificateCategory1,
                                        certType.NameCertificate,
                                        request.RequestStatus,
                                        request.RequestDate
                                    }).ToList();

                    if (requests.Any())
                    {
                        var wsRequests = workbook.Worksheets.Add("طلبات الشهادات");
                        var headers2 = new List<string>
                    {
                        "البطاقة الوطنية", "الاسم الكامل", "صنف الشهادة", "نوع الشهادة", "حالة الطلب", "تاريخ الطلب"
                    };
                        for (int i = 0; i < headers2.Count; i++)
                        {
                            wsRequests.Cell(1, i + 1).Value = headers2[i];
                            wsRequests.Cell(1, i + 1).Style.Font.Bold = true;
                            wsRequests.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        }
                        int row = 2;
                        foreach (var r in requests)
                        {
                            wsRequests.Cell(row, 1).Value = r.NationalID;
                            wsRequests.Cell(row, 2).Value = r.FullName;
                            wsRequests.Cell(row, 3).Value = r.CertificateCategory1;
                            wsRequests.Cell(row, 4).Value = r.NameCertificate;
                            wsRequests.Cell(row, 5).Value = r.RequestStatus;
                            wsRequests.Cell(row, 6).Value = r.RequestDate?.ToString("yyyy-MM-dd");
                            row++;
                        }
                        wsRequests.Columns().AdjustToContents();
                    }

                    //===================== 3️⃣ تصدير الشهادات =====================
                    var certificates = (from certificate in db.Certificate
                                        join request in db.CertificateRequest on certificate.idRequest equals request.id
                                        join citizen in db.Citizen on request.idCitizens equals citizen.id
                                        join certType in db.certificateType on request.idCertificateType equals certType.id
                                        select new
                                        {
                                            citizen.NationalID,
                                            citizen.FullName,
                                            request.RequestStatus,
                                            certType.NameCertificate,
                                            certificate.status,
                                            request.RequestDate,
                                            certificate.date_delivery
                                        }).ToList();

                    if (certificates.Any())
                    {
                        var wsCertificates = workbook.Worksheets.Add("الشهادات");
                        var headers3 = new List<string>
                    {
                        "البطاقة الوطنية", "الاسم الكامل", "حالة الطلب", "نوع الشهادة", "حالة الشهادة", "تاريخ الطلب", "تاريخ التسليم"
                    };
                        for (int i = 0; i < headers3.Count; i++)
                        {
                            wsCertificates.Cell(1, i + 1).Value = headers3[i];
                            wsCertificates.Cell(1, i + 1).Style.Font.Bold = true;
                            wsCertificates.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        }
                        int row = 2;
                        foreach (var cert in certificates)
                        {
                            wsCertificates.Cell(row, 1).Value = cert.NationalID;
                            wsCertificates.Cell(row, 2).Value = cert.FullName;
                            wsCertificates.Cell(row, 3).Value = cert.RequestStatus;
                            wsCertificates.Cell(row, 4).Value = cert.NameCertificate;
                            wsCertificates.Cell(row, 5).Value = cert.status;
                            wsCertificates.Cell(row, 6).Value = cert.RequestDate?.ToString("yyyy-MM-dd");
                            wsCertificates.Cell(row, 7).Value = cert.date_delivery?.ToString("yyyy-MM-dd");
                            row++;
                        }
                        wsCertificates.Columns().AdjustToContents();
                    }

                    //===================== حفظ الملف =====================
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx",
                        Title = "حفظ الملف الشامل",
                        FileName = "ManagementCertificate.xlsx"
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                     
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء التصدير:\n" + ex.Message);
            }
        }
    }
}
