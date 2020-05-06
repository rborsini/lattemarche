using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Interfaces
{
    public interface IStampantiService 
    {
        Task<IEnumerable<Stampante>> GetItemsAsync();

        Task<Stampante> GetDefaultAsync();

        Task<bool> SetDefaultAsync(string macAddress);

        Task<bool> InsertOrUpdateAsync(Stampante stampante);

    }
}
