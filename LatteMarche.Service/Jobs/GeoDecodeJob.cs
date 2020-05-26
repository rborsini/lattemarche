using Autofac;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Application.Utenti.Interfaces;
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
        private IUnitOfWork uow;
        private IAllevamentiService allevamentiService;
        private IComuniService comuniService;
        private IUtentiService utentiService;

        public GeoDecodeJob()
            : base() { }

        public override void Execute()
        {
            using (ILifetimeScope scope = AutoFacConfig.Container.BeginLifetimeScope())
            {
                this.allevamentiService = scope.Resolve<IAllevamentiService>();
                this.comuniService = scope.Resolve<IComuniService>();
                this.utentiService = scope.Resolve<IUtentiService>();

                var records = this.allevamentiService.Search(new Application.Allevamenti.Dtos.AllevamentiSearchDto() { });

                int i = 0;

                foreach(var record in records)
                {
                    var allevamento = this.allevamentiService.Details(record.Id);
                    var utente = this.utentiService.Details(record.IdUtente);

                    if(!allevamento.Latitudine.HasValue)
                    {
                        var comune = this.comuniService.Details(record.IdComune);
                        var indirizzo = $"{record.IndirizzoAllevamento} {comune.CAP} {comune.Descrizione} {comune.Provincia}";

                        var coordinate = Geocoder.Geocode(indirizzo);

                        if (coordinate != null && coordinate.Latitude.HasValue && coordinate.Longitude.HasValue)
                        {
                            allevamento.Latitudine = coordinate.Latitude;
                            allevamento.Longitudine = coordinate.Longitude;

                            this.allevamentiService.Update(allevamento);
                        }

                        Console.WriteLine($"{i} di {records.Count}    {indirizzo} [{coordinate.Latitude} {coordinate.Longitude}]");

                    }



                    i++;
                }

                Console.ReadKey();

            }
        }
    }
}
