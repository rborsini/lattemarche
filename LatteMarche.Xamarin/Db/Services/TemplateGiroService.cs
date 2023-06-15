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
    public class TemplateGiroService : BaseEntityService<TemplateGiro, int>, ITemplateGiroService
    {
        public async Task<IEnumerable<TemplateGiro>> GetItemsAsync(int idTrasportatore)
        {
            using (var context = CreateContext())
            {
                return await context.Set<TemplateGiro>()
                    .Where(t => t.IdTrasportatore == idTrasportatore)
                    .ToListAsync();
            }
        }
    }
}
