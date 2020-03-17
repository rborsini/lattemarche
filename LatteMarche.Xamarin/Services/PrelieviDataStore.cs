using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LatteMarche.Xamarin.Services
{
    public class PrelieviDataStore : BaseDbDataStore<Prelievo, string>
    {

        public PrelieviDataStore()
        { }

        protected override Prelievo UpdateProperties(Prelievo entityItem, Prelievo viewItem)
        {
            entityItem.Scomparto = viewItem.Scomparto;
            entityItem.Quantita = viewItem.Quantita;

            return entityItem;
        }
    }
}
