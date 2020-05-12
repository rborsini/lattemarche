using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Makers.ZPL
{
    public class RegistroConsegnaMaker : AbstractLabelMaker
    {
        public override string MakeLabel(Registro registro)
        {
            var registroConsegna = registro as RegistroConsegna;

            int y = 0;
            var cmd = "";

            // Apro il file con "^XA"
            cmd += this.start_print;

            // Header
            cmd += MakeHeader();

            // Linea
            cmd += MakeLine(ref y);

            // Colonna sx Acquirente
            cmd += MakeAcquirenteSection(registroConsegna, ref y);

            // Colonna dx Destinatario
            cmd += MakeDestinatarioSection(registroConsegna, ref y);

            // Chiudo il file con "^XZ"
            cmd += this.end_print;

            return cmd;
        }
    }
}
