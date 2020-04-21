using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Interfaces
{
    public interface ITrasportatoriService : IEntityService<Trasportatore, int>
    {
        Task<Trasportatore> GetCurrent();
    }
}
