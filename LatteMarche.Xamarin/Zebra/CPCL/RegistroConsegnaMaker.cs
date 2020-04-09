using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.CPCL
{
    public class RegistroConsegnaMaker : AbstractLabelMaker
    {
        public override string MakeLabel(Registro registro)
        {
            var registroRaccolta = registro as RegistroConsegna;

            int y = 0;
            var cmd = "";

            // intestazione
            cmd += MakeHeader(registroRaccolta, ref y);

            // titolo
            cmd += MakerTitle(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Firme
            cmd += MakeFirmeSection("Firma Produttore \\ Delegato", "Firma Trasportatore", ref y);

            // intestazione label aggiunta alla fine per avere l'altezza corretta
            var height = y + 100;
            cmd = $"! {offset} 200 200 {height} {quantity}\r\n" + cmd;
            //cmd += "FORM\r\n";
            cmd += "PRINT\r\n";

            return cmd;

        }

    }
}
