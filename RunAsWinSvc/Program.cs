﻿using System.ServiceProcess;

namespace RunAsWinSvc
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new RunAsSvc()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
