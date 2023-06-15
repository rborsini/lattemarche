using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Interfaces
{
    public interface ITemplateGiroService : IEntityService<TemplateGiro, int>
    {
        Task<IEnumerable<TemplateGiro>> GetItemsAsync(int idTrasportatore);
    }
}
