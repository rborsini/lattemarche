using Autofac;
using LatteMarche.Application.Synch.Interfaces;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LatteMarche.SynchJob
{
    public class Functions
    {
        //private static int DepthDays { get { return Convert.ToInt32(ConfigurationManager.AppSettings["days_depth"]); } }
        //private static string connectionString = ConfigurationManager.ConnectionStrings["OldDbContext"].ConnectionString;
        //private static string baseUrl = ConfigurationManager.AppSettings["ClientSettingsProvider.ServiceUri"];

        /// <summary>
        /// Sincronizzazione nuovo e vecchio server
        /// </summary>
        /// <param name="timerInfo"></param>
        /// <param name="log"></param>
        public static void SynchJob([TimerTrigger("00:00:01")] TimerInfo timerInfo, TextWriter log)
        {
            log.WriteLine("SynchJob started");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (ILifetimeScope scope = AutoFacConfig.Container.BeginLifetimeScope())
            {
                ISynchService service = scope.Resolve<ISynchService>();

                service.Push();

            }

            sw.Stop();

            log.WriteLine("Synch job completed in " + sw.Elapsed);
        }



    }
}
