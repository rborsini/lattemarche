using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zebra.Sdk.Comm;

namespace LatteMarche.Xamarin.Zebra.Interfaces
{
    public interface IPrinter
    {
        string MacAddress { get; set; }

        Task PrintLabel(Registro registro);

    }
}
