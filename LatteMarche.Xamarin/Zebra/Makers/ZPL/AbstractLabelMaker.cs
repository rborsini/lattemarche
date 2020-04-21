using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Makers.ZPL
{
    public abstract class AbstractLabelMaker : ILabelMaker
    {
        public abstract string MakeLabel(Registro registro);
    }
}
