﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagmentSystemCertificate.Admins;
using ManagmentSystemCertificate.Employes;

namespace ManagmentSystemCertificate
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DashbordAdmins());
        }
    }
}
