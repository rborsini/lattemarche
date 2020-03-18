using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra
{
    public class ZplLabelMaker : ILabelMaker
    {
        public string MakeLabel(RegistroConsegna registro)
        {
            return @"^XA
                    ^FO17,16
                    ^GB379,371,8^FS
                    ^FT65,255
                    ^A0N,135,134
                    ^FDTEST^FS
                    ^XZ";
        }

        public string MakeLabel(RegistroRaccolta registro)
        {
            return @"^XA
                    ^FO17,16
                    ^GB379,371,8^FS
                    ^FT65,255
                    ^A0N,135,134
                    ^FDTEST^FS
                    ^XZ";
        }
    }
}
