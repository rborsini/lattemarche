using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LatteMarche.Xamarin.Services
{
    public class GiriDataStore : BaseDbDataStore<Giro, int>, IGiriService
    {
        public async Task<IEnumerable<Giro>> GetGiriTrasportatore(int idTrasportatore)
        {
            using (var context = CrateContext())
            {
                return await context.Set<Giro>()
                    .Where(g => g.IdTrasportatore.HasValue && g.IdTrasportatore.Value == idTrasportatore)
                    .ToListAsync();
            }
        }
    }
}
