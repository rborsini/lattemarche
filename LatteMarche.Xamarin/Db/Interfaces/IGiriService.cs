using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Interfaces
{
    public interface IGiriService : IEntityService<Giro, int>
    {
        Task<IEnumerable<Giro>> GetGiriApertiAsync();

        Task<IEnumerable<Giro>> GetGiriNonArchiviatiAsync();

        Task<bool> ArchiviaGiroPrecedenteAsync(int templateGiro);

    }
}
