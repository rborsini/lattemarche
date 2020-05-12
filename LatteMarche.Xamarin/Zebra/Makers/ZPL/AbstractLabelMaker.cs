using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Makers.ZPL
{
    public abstract class AbstractLabelMaker : ILabelMaker
    {
        public int leftOffset = 50;
        public int h2 = 22;
        public int h3 = 18;
        public int h6 = 12;
        public string start_print = "^XA";
        public string end_print = "^XZ";

        public abstract string MakeLabel(Registro registro);

        protected string MakeHeader()
        {
            var header_line_1 = $"^CFA,{h2}^FO{leftOffset},50^FDLatte Marche^FS";
            var header_line_2 = $"^CFA,{h6}^FO{leftOffset},80^FDOrganizzazione Produttori^FS";
            var header_line_3 = $"^CFA,{h2}^FO{leftOffset},160^FDRegistro consegna latte bovino^FS";
            var header_line_4 = $"^CFA,{h6}^FO{leftOffset},200^FDL. 119/03-D.M. 31/07/03, art.12 - Documentazione raccolta latte - Sistema Informatizzato di registrazione - ^FS";
            var header_line_5 = $"^CFA,{h6}^FO{leftOffset},220^FDAutorizzazione Regione Marche DDS 512/SAR^FS";

            return header_line_1 + header_line_2 + header_line_3 + header_line_4 + header_line_5;
        }        

        protected string MakeLine(ref int y)
        {
            y += 250;
            var cmd = $"^FO{leftOffset},{y}^GB700,1,1^FS";
            return cmd;
        }

        protected string MakeAcquirenteDestinatarioSection(Registro registro, ref int y)
        {
            var cmd = "";
            y += 30;
            // Stringa fissa Acquirente
            cmd += $"^CFA,{h3}^FO{leftOffset},{y}^FDAcquirente^FS";
            // Stringa fissa Destinatario
            cmd += $"^CFA,{h3}^FO500,{y}^FDDestinatario^FS";

            return cmd;
        }

    }
}
