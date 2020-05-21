using LatteMarche.Xamarin.Db;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Services
{
    public class AllevamentiService : BaseEntityService<Allevamento, int>, IAllevamentiService
    {
        public async Task<IEnumerable<Allevamento>> GetByTemplate(int idTemplateGiro)
        {
            using (var context = CreateContext())
            {
                return await context.Set<Allevamento>()
                    .Where(g => g.IdTemplateGiro.HasValue && g.IdTemplateGiro.Value == idTemplateGiro)
                    .ToListAsync();
            }
        }


        public async override Task<Allevamento> GetItemAsync(int id)
        {
            using (var context = CreateContext())
            {
                return await context.Set<Allevamento>()
                    .Include(l => l.TipoLatte)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.IdAllevamento.Equals(id));
            }
        }

    }
}
