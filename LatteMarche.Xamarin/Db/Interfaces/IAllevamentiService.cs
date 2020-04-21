using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Interfaces
{
    public interface IAllevamentiService : IEntityService<Allevamento, int>
    {
        Task<IEnumerable<Allevamento>> GetByTemplate(int idTemplateGiro);
    }
}
