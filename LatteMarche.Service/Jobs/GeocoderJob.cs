using Autofac;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Core.Models;
using LatteMarche.Utils.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Service.Jobs
{
    public class GeoDecodeJob : BaseJob
    {

        public GeoDecodeJob()
            : base() { }

        public override void Execute()
        {
            var sql = "";

            using (ILifetimeScope scope = AutoFacConfig.Container.BeginLifetimeScope())
            {

                var uow = scope.Resolve<IUnitOfWork>();
                var allevamentiRepo = uow.Get<Allevamento, int>();

                var allevamenti = allevamentiRepo.DbSet.Where(a => !a.Latitudine.HasValue).ToList();

                int i = 1;

                foreach (var allevamento in allevamenti)
                {

                    var indirizzo = $"{allevamento.IndirizzoAllevamento} {allevamento.Comune.CAP} {allevamento.Comune.Descrizione} {allevamento.Comune.Provincia}";

                    var coordinate = Geocoder.Geocode(indirizzo);

                    if (coordinate != null && coordinate.Latitude.HasValue && coordinate.Longitude.HasValue)
                    {
                        sql += $"update ANAGRAFE_ALLEVAMENTO SET Latitudine = {coordinate.Latitude}, Longitudine = {coordinate.Longitude} where ID_ALLEVAMENTO = {allevamento.Id} \n";
                    }                    

                    Console.WriteLine($"{i} di {allevamenti.Count}    {indirizzo} [{coordinate.Latitude} {coordinate.Longitude}]");

                    i++;
                }

                Console.WriteLine("FATTO");
                Console.ReadKey();

            }
        }
    }
}
