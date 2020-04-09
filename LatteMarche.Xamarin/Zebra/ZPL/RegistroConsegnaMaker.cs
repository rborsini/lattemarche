using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.ZPL
{
    public class RegistroConsegnaMaker : AbstractLabelMaker
    {
        public override string MakeLabel(Registro registro)
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
