using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.CPCL
{
    public abstract class AbstractLabelMaker : ILabelMaker
    {
        protected const int WIDTH = 95;

        protected int quantity = 1;           // numero copie
        protected int offset = 0;             // offset sx label
        protected int x = 30;                 // margine sx singola riga
        protected string h1 = "7 1";          // H1 => font 7 size 1
        protected string p = "0 2";           // P  => font 0 size 2

        public abstract string MakeLabel(Registro registro);

        protected string MakeHeader(Registro registro, ref int y)
        {
            var cmd = "";

            cmd += $"TEXT {h1} {x} 0 {registro.Header_1} \r\n";         // latte marche
            cmd += $"TEXT {p} {x} 50 {registro.Header_2} \r\n";         // Organizzazione Produttori
            cmd += $"TEXT {h1} {x} 100 {registro.Titolo} \r\n";         // Registro raccolta latte bovino

            y = 150;

            return cmd;
        }

        protected string MakerTitle(Registro registro, ref int y)
        {
            var cmd = "";

            // Sottotitolo - L.119/03-D.M. 31/07/03, art.12 - Documentazione raccolta latte - Sistema informatizzato di registrazione - Autorizzazione Regione Marche DDs 512/SAR
            y += 10;
            foreach (var riga in SplitInLine(registro.SottoTitolo, WIDTH))
            {
                cmd += $"TEXT {p} {x} {y} {riga} \r\n";
                y += 25;
            }

            y += 10;

            return cmd;
        }

        protected string MakeFirmeSection(string firmaSx, string firmaDx, ref int y)
        {
            var cmd = "";

            y += 20;

            // Firme
            cmd += $"TEXT {p} {x} {y} {PadRight(firmaSx, 50)} {PadRight(firmaDx, 50)} \r\n";
            y += 100;

            // Linee
            cmd += $"LINE 30 {y} 280 {y} 2 \r\n";

            cmd += $"LINE 430 {y} 680 {y} 2 \r\n";

            return cmd;
        }

        protected string MakeLine(ref int y)
        {
            var cmd = $"LINE 0 {y} 800 {y} 2 \r\n";
            y += 20;

            return cmd;
        }

        protected List<string> SplitInLine(string str, int chunkSize)
        {
            var lines = new List<string>();

            var size = str.Length / chunkSize;
            if (str.Length % chunkSize != 0)
                size += 1;

            for (var i = 0; i < size; i++)
            {
                var start = i * chunkSize;
                var length = start + chunkSize > str.Length ? str.Length - start : chunkSize;

                lines.Add(str.Substring(start, length));
            }

            return lines;
        }

        protected string PadRight(string source, int length, char paddingChar = ' ')
        {
            var result = String.Empty;

            if (source.Length > length)
                result = source.Substring(0, length);

            if (source.Length < length)
                result = source.PadRight(length, ' ');

            return result;
        }

    }
}
