using AutoMapper;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Rest.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Services
{
    public class TrasbordiService : ITrasbordiService
    {
        public async Task Import(TrasbordoDto trasbordo)
        {
            using (var context = (LatteMarcheDbContext)Activator.CreateInstance(typeof(LatteMarcheDbContext)))
            {
                var templateGiro = await context.Set<TemplateGiro>().FindAsync(trasbordo.IdTemplateGiro);
                if (templateGiro == null)
                    return;

                var prelievi = Mapper.Map<List<Prelievo>>(trasbordo.Prelievi);

                if(prelievi.Count > 0)
                {
                    var allevamento = await context.Set<Allevamento>().FindAsync(prelievi[0].IdAllevamento);
                    var tipoLatte = await context.Set<TipoLatte>().FindAsync(allevamento.IdTipoLatte);
                    
                    foreach(var prelievo in prelievi)
                    {
                        prelievo.Quantita_lt = prelievo.Quantita_kg * tipoLatte.FattoreConversione.Value;
                    }
                }

                var giro = new Giro()
                {
                    DataCreazione = DateTime.Now,
                    IdTemplateGiro = trasbordo.IdTemplateGiro,
                    Titolo = templateGiro.Descrizione,
                    LavaggioCisterna = true,
                    Prelievi = prelievi
                };

                await context.Set<Giro>().AddAsync(giro);

                await context.SaveChangesAsync();
                await Task.CompletedTask;
            }


        }
    }
}
