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
        private int quantity = 1;           // numero copie
        private int offset = 0;             // offset sx label
        private int x = 50;                 // margine sx singola riga
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

            // Firme
            cmd += MakeFirmeSection(ref y);

            // intestazione label aggiunta alla fine per avere l'altezza corretta
            var height = y + 100;
            cmd = $"! {offset} 200 200 {height} {quantity}\r\n" + cmd;
            //cmd += "FORM\r\n";
            cmd += "PRINT\r\n";

            return cmd;

        }

        public string MakeLabel(RegistroConsegna registro)
        {
            return @"! 0 200 200 406 1\r\n" +
                "ON-FEED IGNORE\r\n" +
                "BOX 20 20 380 380 8\r\n" +
                "T 0 6 137 177 TEST\r\n" +
                "PRINT\r\n";
        }

        private string MakeHeader(Registro registro, ref int y)
        {
            var cmd = "";

            cmd += $"TEXT {h1} {x} 0 {registro.Header_1} \r\n";         // latte marche
            cmd += $"TEXT {p} {x} 50 {registro.Header_2} \r\n";         // Organizzazione Produttori
            cmd += $"TEXT {h1} {x} 100 {registro.Titolo} \r\n";         // Registro raccolta latte bovino

            y = 150;

            return cmd;
        }

        private string MakerTitle(Registro registro, ref int y)
        {
            var cmd = "";

            // Sottotitolo - L.119/03-D.M. 31/07/03, art.12 - Documentazione raccolta latte - Sistema informatizzato di registrazione - Autorizzazione Regione Marche DDs 512/SAR
            y += 10;
            foreach (var riga in SplitInLine(registro.SottoTitolo, 95))
            {
                cmd += $"TEXT {p} {x} {y} {riga} \r\n";
                y += 25;
            }

            y += 10;

            return cmd;
        }

        private string MakeLine(ref int y)
        {
            var cmd = $"LINE 0 {y} 800 {y} 2 \r\n";
            y += 20;

            return cmd;
        }

        private string MakeAcquirenteDestinatarioSection(Registro registro, ref int y)
        {
            var cmd = "";

            int sxColWidth = 95/2;
            int lineSpacing = 30;

            // intestazione
            var headerAcq = PadRight("Acquirente", sxColWidth, ' ');
            var headerDest = PadRight("Destinatario", sxColWidth, ' ');
            y += 20;
            cmd += $"TEXT {p} {x} {y} {headerAcq} {headerDest}\r\n";

            // ragioni sociali
            var rsAcq = PadRight(registro.Acquirente.RagioneSociale, sxColWidth, ' ');
            var rsDest = PadRight(registro.Destinatario.RagioneSociale, sxColWidth, ' ');
            y += lineSpacing;
            cmd += $"TEXT {p} {x} {y} {rsAcq} {rsDest}\r\n";

            // indirizzi
            var indAcq = PadRight(registro.Acquirente.Indirizzo, sxColWidth, ' ');
            var indDest = PadRight(registro.Destinatario.Indirizzo, sxColWidth, ' ');
            y += lineSpacing;
            cmd += $"TEXT {p} {x} {y} {indAcq} {indDest}\r\n";

            // cap / comune / prov
            var comAcq = PadRight($"{registro.Acquirente.CAP} {registro.Acquirente.Comune} ({registro.Acquirente.Provincia})", sxColWidth, ' ');
            var comDest = PadRight($"{registro.Destinatario.CAP} {registro.Destinatario.Comune} ({registro.Destinatario.Provincia})", sxColWidth, ' ');
            y += lineSpacing;
            cmd += $"TEXT {p} {x} {y} {comAcq} {comDest}\r\n";

            // P IVA
            var pivaAcq = PadRight($"P.IVA {registro.Acquirente.P_IVA}", sxColWidth, ' ');
            var pivaDest = PadRight($"P.IVA {registro.Destinatario.P_IVA}", sxColWidth, ' ');
            y += lineSpacing;
            cmd += $"TEXT {p} {x} {y} {pivaAcq} {pivaDest}\r\n";

            y += lineSpacing;

            return cmd;
        }

        private string MakeTrasportatoreSection(Registro registro, ref int y)
        {
            var cmd = "";

            int lineSpacing = 30;

            // Targa
            cmd += $"TEXT {p} {x} {y} Targa automezzo: {registro.Trasportatore.TargaAutomezzo}\r\n";
            y += lineSpacing;

            // Trasportatore
            cmd += $"TEXT {p} {x} {y} Trasportatore: {registro.Trasportatore.RagioneSociale}\r\n";
            y += lineSpacing;

            // Indirizzo
            cmd += $"TEXT {p} {x} {y} {registro.Trasportatore.Indirizzo}\r\n";
            y += lineSpacing;

            // P IVA
            cmd += $"TEXT {p} {x} {y} P.IVA: {registro.Trasportatore.P_IVA}\r\n";
            y += lineSpacing;

            return cmd;
        }

        private string MakeLottoSection(RegistroRaccolta registro, ref int y)
        {
            var cmd = "";

            int sxColWidth = 25;
            int lineSpacing = 50;

            // Data / Giro
            var data = PadRight($"Data: {registro.Data.ToString("dd/MM/yyyy")}", sxColWidth, ' ');
            var giro = PadRight($"Giro: {registro.Giro.Nome}", sxColWidth, ' ');            
            cmd += $"TEXT {h1} {x} {y} {data} {giro}\r\n";
            y += lineSpacing;

            // Ora / Lotto
            var ora = PadRight($"Ora: {registro.Data.ToString("HH:mm")}", sxColWidth, ' ');
            var lotto = PadRight($"Lotto: {registro.Lotto.Codice}", sxColWidth, ' ');            
            cmd += $"TEXT {h1} {x} {y} {ora} {lotto}\r\n";
            y += lineSpacing + (lineSpacing / 2);       // interlinea 1.5 

            return cmd;
        }

        private string MakeScompartoSection(RegistroRaccolta registro, ref int y)
        {
            var cmd = "";

            int lineSpacing = 30;

            // Latte bovino
            cmd += $"TEXT {p} {x} {y} Latte bovino crudo convenzionale scomparto N. 1 2 3 4 \r\n";
            y += (lineSpacing * 2);

            foreach (var riga in SplitInLine(registro.Comunicazioni, 95))
            {
                cmd += $"TEXT {p} {x} {y} {riga} \r\n";
                y += lineSpacing;
            }

            cmd += $"TEXT {p} {x} {y} scomparto N. 1 2 3 4 \r\n";
            y += (lineSpacing * 2);

            return cmd;
        }

        private string MakeFirmeSection(ref int y)
        {
            var cmd = "";

            // Firme
            cmd += $"TEXT {p} {x} {y} Firma Acquirente \\ Delegato                   Firma Destinatario \r\n";
            y += 100;

            // Linee
            cmd += $"LINE 30 {y} 280 {y} 2 \r\n";

            cmd += $"LINE 430 {y} 680 {y} 2 \r\n";

            return cmd;
        }

        private List<string> SplitInLine(string str, int chunkSize)
        {
            var lines = new List<string>();

            var size = str.Length / chunkSize;
            if (str.Length % chunkSize != 0)
                size += 1;

            for(var i = 0; i< size; i++)
            {
                var start = i * chunkSize;
                var length = start + chunkSize > str.Length ? str.Length - start : chunkSize;

                lines.Add(str.Substring(start, length));
            }

            return lines;
        }

        private string PadRight(string source, int length, char paddingChar)
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
