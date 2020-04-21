using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Interfaces
{
    interface IPrelieviService : IEntityService<Prelievo, string>
    {

        Task<IEnumerable<Prelievo>> GetByGiro(int idGiro);

    }
}
