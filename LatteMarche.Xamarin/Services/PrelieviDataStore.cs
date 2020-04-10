using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LatteMarche.Xamarin.Services
{
    public class PrelieviDataStore : BaseDbDataStore<Prelievo, string>
    {

        public PrelieviDataStore()
        { }

        public async Task<IEnumerable<Prelievo>> GetItemsAsync(string idLotto)
        {
            using (var context = CrateContext())
            {
                return await context.Set<Prelievo>()
                    .Where(p => p.IdLotto == idLotto)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        protected override Prelievo UpdateProperties(Prelievo entityItem, Prelievo viewItem)
        {
            entityItem.Scomparto = viewItem.Scomparto;
            entityItem.Quantita_kg = viewItem.Quantita_kg;

            return entityItem;
        }
    }
}
