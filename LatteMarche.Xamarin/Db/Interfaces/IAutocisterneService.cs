using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Interfaces
{
    public interface IAutoCisterneService : IEntityService<AutoCisterna, int>
    {
        Task<AutoCisterna> GetDefaultAsync();

        Task<bool> SetDefaultAsync(int idAutocisterna);
    }
}
