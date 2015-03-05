using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Core.Windows.Services.Configuration
{
    public class WindowsServiceConfig : ConfigurationSection
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        public static WindowsServiceConfig Section(string section = "windowsService")
        {
            return ConfigurationManager.GetSection(section) as WindowsServiceConfig;
        }
    }
}
