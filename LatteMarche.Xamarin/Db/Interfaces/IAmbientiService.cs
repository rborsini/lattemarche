using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Db.Interfaces
{
    public interface IAmbientiService : IEntityService<Ambiente, int>
    {
        List<Ambiente> Init();

        void SetDefault(int idAmbiente);

        Ambiente GetDefault();
    }
}
