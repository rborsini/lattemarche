using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LatteMarche.Xamarin.Services
{
    public class LottiDataStore : BaseDbDataStore<Lotto, string>
    {

        public LottiDataStore()
        { }

        protected override Lotto UpdateProperties(Lotto entityItem, Lotto viewItem)
        {

            return entityItem;
        }
    }
}
