using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Interfaces
{
    public interface ILabelMaker
    {
        string MakeLabel(Registro registro);
    }
}
