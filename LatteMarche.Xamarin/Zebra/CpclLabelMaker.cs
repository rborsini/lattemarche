using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LatteMarche.Xamarin.Zebra
{
    public class CpclLabelMaker : ILabelMaker
    {
        private const int WIDTH = 95;

        private int quantity = 1;           // numero copie
        private int offset = 0;             // offset sx label
        private int x = 30;                 // margine sx singola riga
        private string h1 = "7 1";          // H1 => font 7 size 1
        private string p = "0 2";           // P  => font 0 size 2

        public string MakeLabel(RegistroRaccolta registro)
        {

            int y = 0;
            var cmd = "";

            // intestazione
            cmd += MakeHeader(registro, ref y);

            // titolo
            cmd += MakerTitle(registro, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Acquirente / Destinatario
            cmd += MakeAcquirenteDestinatarioSection(registro, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Automezzo
            cmd += MakeTrasportatoreSection(registro, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // lotto 
            cmd += MakeLottoSection(registro, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // scomparto
            cmd += MakeScompartoSection(registro, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // tabella scomparti
            cmd += MakeTabellaSection(registro, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Firme
            cmd += MakeFirmeSection(ref y);

            // intestazione label aggiunta alla fine per avere l'altezza corretta
            var height = y + 100;
            cmd = $"! {offset} 200 200 {height} {quantity}\r\n" + cmd;
            //cmd += "FORM\r\n";
            cmd += "PRINT\r\n";

            return cmd;

        }



    }
}
