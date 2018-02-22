using Autofac;
using LatteMarche.Application.Lotti.Interfaces;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Sitra.Interfaces;
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
                ISynchService synchService = scope.Resolve<ISynchService>();
                ISitraService sitraService = scope.Resolve<ISitraService>();
                ILottiService lottiService = scope.Resolve<ILottiService>();

                // scarica i dati dal cloud verso server locale
                //synchService.Pull();

                // carica i dati locali verso il cloud
                var nuoviPrelievi = synchService.Push();

                //// estrazione lotti dai nuovi prelievi
                //var lotti = lottiService.GetLotti(nuoviPrelievi);

                //// invio lotti Sitra
                //var lottiAggiornati = sitraService.InvioLotti(lotti);

                //// persistenza database dei lotti inviati
                //foreach(var lotto in lottiAggiornati)
                //{
                //    lottiService.Create(lotto);
                //}


            }

            sw.Stop();

            log.WriteLine("Synch job completed in " + sw.Elapsed);
        }

        private static void CallTest()
        {
            Console.Write("TEST OK!!");
            Console.ReadKey();
        }
    }
}
