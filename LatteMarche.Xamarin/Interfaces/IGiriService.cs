using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Interfaces
{
    public interface IGiriService 
    {
        Task<IEnumerable<Giro>> GetGiriTrasportatore(int idTrasportatore);
    }
}
