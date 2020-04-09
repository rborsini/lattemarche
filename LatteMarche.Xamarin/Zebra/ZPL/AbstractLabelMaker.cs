using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.ZPL
{
    public abstract class AbstractLabelMaker : ILabelMaker
    {
        public abstract string MakeLabel(Registro registro);
    }
}
