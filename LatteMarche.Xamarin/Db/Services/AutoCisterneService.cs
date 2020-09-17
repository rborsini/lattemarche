using LatteMarche.Xamarin.Db;
using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Services
{
    public class AutoCisterneService : BaseEntityService<AutoCisterna, int>, IAutoCisterneService
    {
        public async Task<AutoCisterna> GetDefaultAsync()
        {
            using (var context = CreateContext())
            {
                return await context.Set<AutoCisterna>().FirstOrDefaultAsync();
            }
        }
    }
}
