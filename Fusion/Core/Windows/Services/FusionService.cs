using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.Core.Windows.Services
{
    /// <summary>
    /// Represents a fusion service
    /// </summary>
    public class FusionService : ServiceBase
    {
        private Action _action;
        private Assembly _assembly;

        /// <summary>
        /// Raised when the service starts
        /// </summary>
        public event EventHandler OnServiceStart;

        /// <summary>
        /// Raised when the service stops
        /// </summary>
        public event EventHandler OnServiceStop;

        /// <summary>
        /// The display name of this service
        /// </summary>
        public string ServiceDisplayName { get; private set; }
        
        /// <summary>
        /// The description of this service
        /// </summary>
        public string ServiceDescription { get; private set; }

        /// <summary>
        /// Whether to run as a service or not
        /// </summary>
        public bool RunAsService { get; set; }

        public FusionService(string name) : this(Assembly.GetCallingAssembly(), name, name, name) { }
        public FusionService(Assembly assembly, string name, string displayName, string discription)
        {
            RunAsService = true;
            displayName = displayName ?? name;
            ServiceDescription = discription;
            ServiceDisplayName = displayName;
            ServiceName = name;
            _assembly = assembly;
        }

        /// <summary>
        /// Start this service
        /// </summary>
        public void Start(Action onStarted)
        {
            _action = onStarted;
            if (RunAsService && !Debugger.IsAttached)
                ServiceBase.Run(this);
            else
                OnStart(null);
        }

        /// <summary>
        /// Returns whether this service is installed or not
        /// </summary>
        public bool Installed
        {
            get
            {
                return (ServiceController.GetServices().Any(s => s.ServiceName == ServiceName));
            }
        }

        /// <summary>
        /// Install this service
        /// </summary>
        public void Install()
        {
            if (Installed)
                Uninstall();

            var serviceInstaller = new ServiceInstaller();
            var serviceProcessInstaller = new ServiceProcessInstaller();
            serviceInstaller.Parent = serviceProcessInstaller;
            serviceInstaller.DisplayName = ServiceDisplayName;
            serviceInstaller.ServiceName = ServiceName;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.Context = new InstallContext("install.log", null);
            serviceInstaller.Context.Parameters["assemblypath"] = _assembly.Location;
            var stateSaver = new Hashtable();
            serviceInstaller.Install(stateSaver);
        }

        /// <summary>
        /// Uninstall this service
        /// </summary>
        public void Uninstall()
        {
            if (Installed)
            {
                var serviceInstaller = new ServiceInstaller();
                var serviceProcessInstaller = new ServiceProcessInstaller();
                serviceInstaller.Parent = serviceProcessInstaller;
                serviceInstaller.ServiceName = ServiceName;
                serviceInstaller.Context = new InstallContext("uninstall.log", null);
                serviceInstaller.Uninstall(null);
            }
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            if (_action != null)
                _action();
            if (OnServiceStart!=null)
                OnServiceStart(this, new EventArgs());
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (OnServiceStop != null)
                OnServiceStop(this, new EventArgs());
        }
    }
}
