using LatteMarche.Xamarin.Rest.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Interfaces
{
    interface ITrasbordiService
    {
        Task Import(TrasbordoDto trasbordo);
    }
}
