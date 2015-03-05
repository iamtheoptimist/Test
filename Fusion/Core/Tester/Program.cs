using Fusion.Core.Windows;
using Fusion.Core.Windows.Services;
using Fusion.Core.Windows.Services.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tester
{
    public class Program
    {
        static void Main(string[] args)
        {
            var service = new FusionService(WindowsServiceConfig.Section().Name);
            if (!service.Installed)
                service.Install();
            service.Start(() =>
            {
                //MessageBox.Show("hello");
                File.WriteAllText(@"C:\temp\zzz.txt", "fffefwfwefwef");
            });
        }
    }
}
