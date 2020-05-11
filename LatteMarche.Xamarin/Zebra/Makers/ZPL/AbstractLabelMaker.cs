using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Makers.ZPL
{
    public abstract class AbstractLabelMaker : ILabelMaker
    {
        protected int leftOffset = 50;
        protected int h2 = 22;
        protected int h6 = 12;
        public string start_print = "^XA";
        public string end_print = "^XZ";

        public abstract string MakeLabel(Registro registro);



        protected string MakeHeader()
        {
            var header_line_1 = $"^CFA,{h2}^FO{leftOffset},50^FDLatte Marche^FS";
            var header_line_2 = $"^CFA,{h6}^FO{leftOffset},80^FDOrganizzazione Produttori^FS";
            return header_line_1 + header_line_2;
        }        

        protected string MakeLine()
        {
            var cmd = $"^FO{leftOffset},200^GB700,1,1^FS";
            return cmd;
        }
    }
}
