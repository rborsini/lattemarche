using AutoMapper;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Rest.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Services
{
    public class TrasbordiService : ITrasbordiService
    {
        public async Task Import(TrasbordoDto trasbordo, string scomparto)
        {
            using (var context = (LatteMarcheDbContext)Activator.CreateInstance(typeof(LatteMarcheDbContext)))
            {
                var templateGiro = await context.Set<TemplateGiro>().FindAsync(trasbordo.IdTemplateGiro);
                if (templateGiro == null)
                    return;

                var giro = new Giro()
                {
                    DataCreazione = DateTime.Now,
                    IdTemplateGiro = trasbordo.IdTemplateGiro,
                    Titolo = templateGiro.Descrizione,
                    LavaggioCisterna = true
                };
                await context.Set<Giro>().AddAsync(giro);
                await context.SaveChangesAsync();

                var prelievi = Mapper.Map<List<Prelievo>>(trasbordo.Prelievi);
                if(prelievi.Count > 0)
                {
                    foreach(var prelievo in prelievi)
                    {
                        var allevamento = context.Set<Allevamento>().FirstOrDefault(a => a.IdAllevamento == prelievo.IdAllevamento);
                        var tipoLatte = await context.Set<TipoLatte>().FindAsync(allevamento.IdTipoLatte);

                        prelievo.IdGiro = giro.Id;
                        prelievo.Titolo = allevamento.RagioneSociale;
                        prelievo.Quantita_lt = Math.Round(prelievo.Quantita_kg.Value / tipoLatte.FattoreConversione.Value, 1);
                        prelievo.Scomparto = scomparto;
                        prelievo.Trasbordo = $"Trasbordo da {trasbordo.Targa_Origine} a {trasbordo.Targa_Destinazione} del {trasbordo.Data:dd-MM-yyyy}";

                        await context.Set<Prelievo>().AddAsync(prelievo);
                    }
                }

                await context.SaveChangesAsync();
                await Task.CompletedTask;
            }


        }
    }
}
