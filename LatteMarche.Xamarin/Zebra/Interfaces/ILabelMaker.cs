using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Interfaces
{
    public interface ILabelMaker
    {
        string MakeLabel(Registro registro);
    }
}
