using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LatteMarche.Xamarin.Interfaces;

namespace LatteMarche.Xamarin.Services
{
    public class PrelieviDataStore : BaseDbDataStore<Prelievo, string>, IPrelieviService
    {

        public PrelieviDataStore()
        { }

        public async override Task<Prelievo> GetItemAsync(string id)
        {
            using (var context = CrateContext())
            {
                return await context.Set<Prelievo>()
                    .Include(l => l.TipoLatte)
                    .Include(l => l.Lotto)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id.Equals(id));
            }
        }


        protected override Prelievo UpdateProperties(Prelievo entityItem, Prelievo viewItem)
        {
            entityItem.DataConsegna = viewItem.DataConsegna;
            entityItem.DataPrelievo = viewItem.DataPrelievo;
            entityItem.DataUltimaMungitura = viewItem.DataUltimaMungitura;
            entityItem.IdAllevamento = viewItem.IdAllevamento;
            entityItem.IdTipoLatte = viewItem.IdTipoLatte;
            entityItem.NumeroMungiture = viewItem.NumeroMungiture;
            entityItem.Quantita_kg = viewItem.Quantita_kg;
            entityItem.Quantita_lt = viewItem.Quantita_lt;
            entityItem.Scomparto = viewItem.Scomparto;
            entityItem.Temperatura = viewItem.Temperatura;

            return entityItem;
        }
    }
}
