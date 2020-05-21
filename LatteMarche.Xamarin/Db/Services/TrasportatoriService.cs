using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Db.Interfaces;

namespace LatteMarche.Xamarin.Db.Services
{
    public class TrasportatoriService : BaseEntityService<Trasportatore, int>, ITrasportatoriService
    {
        public async Task<Trasportatore> GetCurrent()
        {
            using (var context = CreateContext())
            {
                return await context
                    .Set<Trasportatore>()
                    .Include(t => t.AutoCisterna)
                    .FirstOrDefaultAsync();
            }
        }

        protected override Trasportatore UpdateProperties(Trasportatore entityItem, Trasportatore viewItem)
        {
            entityItem.RagioneSociale = viewItem.RagioneSociale;
            entityItem.Indirizzo = viewItem.Indirizzo;
            entityItem.P_IVA = viewItem.P_IVA;

            return entityItem;
        }

    }
}
