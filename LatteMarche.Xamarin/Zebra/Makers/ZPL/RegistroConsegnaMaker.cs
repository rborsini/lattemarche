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
            cmd += MakeLine(220);

            // Colonna sx Acquirente
            cmd += MakeAcquirenteSection(registroConsegna, 210);

            // Colonna dx Destinatario
            cmd += MakeDestinatarioSection(registroConsegna, 210);
            
            // Linea
            cmd += MakeLine(410);

            // Dati trasportatore
            cmd += MakeDatiTrasportatore(registroConsegna, 430);

            // Linea
            cmd += MakeLine(560);

            // Chiudo il file con "^XZ"
            cmd += this.end_print;

            return cmd;
        }
    }
}
