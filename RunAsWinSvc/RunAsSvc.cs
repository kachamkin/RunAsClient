using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace RunAsWinSvc
{
    public partial class RunAsSvc : ServiceBase
    {
        private Listener? listener;

        public RunAsSvc()
        {
            InitializeComponent();
            EventLog.Source = "Run As Service";
            CanStop = false;
            CanShutdown = false;
        }

        protected override void OnStart(string[] args)
        {
            string port = "";
            if (args.Length > 0)
                port = args[0];
            else
            {
                string[] commandLine = Environment.GetCommandLineArgs();
                if (commandLine.Length > 1)
                    port = commandLine[1];
            };
            if (string.IsNullOrWhiteSpace(port))
                port = "500";

            try
            {
                listener = new Listener(int.Parse(port));
                Task.Run(() => listener.Start(EventLog));
                EventLog.WriteEntry("Service srarted successfully at port " + port + "!", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Couldn't start service at port " + port + ":\r\n" + ex.Message, EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            listener?.Stop();
        }
    }
}
