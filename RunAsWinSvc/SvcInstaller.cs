//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Configuration.Install;
//using System.Linq;
//using System.ServiceProcess;
//using System.Threading.Tasks;

//namespace RunAsWinSvc
//{
//    [RunInstaller(true)]
//    public partial class SvcInstaller : System.Configuration.Install.Installer
//    {
//        readonly ServiceInstaller serviceInstaller;
//        readonly ServiceProcessInstaller processInstaller;
//        public SvcInstaller()
//        {
//            InitializeComponent();

//            processInstaller = new()
//            {
//                Account = ServiceAccount.LocalSystem 
//            };

//            serviceInstaller = new()
//            {
//                StartType = ServiceStartMode.Automatic, 
//                ServiceName = "Run As Service"
//            };

//            Installers.Add(processInstaller);
//            Installers.Add(serviceInstaller);
//        }

//        protected override void OnAfterInstall(IDictionary savedState)
//        {
//            base.OnAfterInstall(savedState);

//            using ServiceController sc = new("Run As Service");
//            sc.Start(new string[] { "500" });
//            sc.Close();
//        }

//    }
//}
