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
            using (var context = CreateContext())
            {
                return await context.Set<Giro>()
                    .Where(g => !g.DataConsegna.HasValue)
                    .Include(g => g.Prelievi)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Giro>> GetGiriNonArchiviatiAsync()
        {
            using (var context = CreateContext())
            {
                return await context.Set<Giro>()
                    .Where(g => !g.Archiviato)
                    .Include(g => g.Prelievi)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async override Task<Giro> GetItemAsync(int id)
        {
            using (var context = CreateContext())
            {
                return await context.Set<Giro>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id.Equals(id));
            }
        }

        public override async Task<bool> DeleteItemAsync(int id)
        {
            using (var context = CreateContext())
            {
                var existingGiro = context.Set<Giro>()
                    .Include(g => g.Prelievi)
                    .FirstOrDefaultAsync(p => p.Id.Equals(id))
                    .Result;

                context.Remove(existingGiro);
                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
        }

        public async Task<bool> ArchiviaGiroPrecedenteAsync(int templateGiro)
        {
            using (var context = CreateContext())
            {
                // ricerca giro precedente
                var giroPrecedente = context.Set<Giro>()
                    .Where(p => p.IdTemplateGiro.Value == templateGiro && p.DataConsegna.HasValue && !p.Archiviato)
                    .FirstOrDefaultAsync()
                    .Result;

                if(giroPrecedente != null)
                {
                    giroPrecedente.Archiviato = true;
                    context.Update<Giro>(giroPrecedente);

                    await context.SaveChangesAsync();
                }

                return await Task.FromResult(true);
            }


        }

        protected override Giro UpdateProperties(Giro entityItem, Giro viewItem)
        {
            entityItem.CodiceLotto = viewItem.CodiceLotto;
            entityItem.Titolo = viewItem.Titolo;
            entityItem.DataConsegna = viewItem.DataConsegna;
            entityItem.DataUpload = viewItem.DataUpload;
            entityItem.LavaggioCisterna = viewItem.LavaggioCisterna;

            return entityItem;
        }


    }
}
