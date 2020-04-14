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
    public class LottiDataStore : BaseDbDataStore<Lotto, string>, ILottiService
    {

        public LottiDataStore()
        { }

        public async override Task<Lotto> GetItemAsync(string id)
        {
            using (var context = CrateContext())
            {
                return await context.Set<Lotto>()
                    .Include(l => l.Prelievi)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id.Equals(id));
            }
        }

        protected override Lotto UpdateProperties(Lotto entityItem, Lotto viewItem)
        {

            return entityItem;
        }
    }
}
