using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Makers.ZPL
{
    public class RegistroRaccoltaMaker : AbstractLabelMaker
    {
        public override string MakeLabel(Registro registro)
        {
            var registroRaccolta = registro as RegistroRaccolta;

            var cmd = "";

            // Apro il file con "^XA"
            cmd += this.start_print;

            // Header
            cmd += MakeHeader();

            // Linea
            cmd += MakeLine();

            // Chiudo il file con "^XZ"
            cmd += this.end_print;

            return cmd;
        }
    }
}
