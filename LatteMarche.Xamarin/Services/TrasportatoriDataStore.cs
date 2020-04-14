using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LatteMarche.Xamarin.Interfaces;

namespace LatteMarche.Xamarin.Services
{
    public class TrasportatoriDataStore : BaseDbDataStore<Trasportatore, int>, ITrasportatoriService
    {
        public async Task<Trasportatore> GetSelected()
        {
            using (var context = CrateContext())
            {
                return await context.Set<Trasportatore>().FirstOrDefaultAsync(i => i.Selezionato);
            }
        }
    }
}
