using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LatteMarche.Xamarin.Db;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.Db.Services
{
    public class GiriService : BaseEntityService<Giro, int>, IGiriService
    {

        public async Task<IEnumerable<Giro>> GetGiriApertiAsync()
        {
            using (var context = CrateContext())
            {
                return await context.Set<Giro>()
                    .Where(g => !g.DataUpload.HasValue)
                    .Include(g => g.Prelievi)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async override Task<Giro> GetItemAsync(int id)
        {
            using (var context = CrateContext())
            {
                return await context.Set<Giro>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id.Equals(id));
            }
        }

        public override async Task<bool> DeleteItemAsync(int id)
        {
            using (var context = CrateContext())
            {
                var existingPrelievo = context.Set<Giro>()
                    .Include(g => g.Prelievi)
                    .FirstOrDefaultAsync(p => p.Id.Equals(id))
                    .Result;

                context.Remove(existingPrelievo);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        protected override Giro UpdateProperties(Giro entityItem, Giro viewItem)
        {
            entityItem.CodiceLotto = viewItem.CodiceLotto;
            entityItem.Titolo = viewItem.Titolo;
            entityItem.DataConsegna = viewItem.DataConsegna;
            entityItem.DataUpload = viewItem.DataUpload;

            return entityItem;
        }


    }
}
