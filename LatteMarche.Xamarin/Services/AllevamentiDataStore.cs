using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Services
{
    public class AllevamentiDataStore : BaseDbDataStore<Allevamento, int>
    {
        protected override Allevamento UpdateProperties(Allevamento entityItem, Allevamento viewItem)
        {
            return entityItem;
        }
    }
}
