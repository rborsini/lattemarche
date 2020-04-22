using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Db.Interfaces
{
    public interface ISincronizzazioneService 
    {
        Task<bool> AddAsync(SynchType tipo);
    }
}
