using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Makers.CPCL
{
    /// <summary>
    /// Classe base per la generazione delle ricevute di consegna e raccolta latte
    /// </summary>
    public abstract class AbstractLabelMaker : ILabelMaker
    {
        protected const int WIDTH = 98;         // Larghezza ricevuta in caratteri

        protected int offset = 0;               // offset sx label
        protected int x = 30;                   // margine sx singola riga
        protected string h1 = "7 1";            // H1 => font 7 size 1
        protected string p = "0 2";             // P  => font 0 size 2
        protected string b = "0 3";             // P  => font 0 size 3

        /// <summary>
        /// Generazione comando CPCL
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        public abstract string MakeLabel(Registro registro);

        /// <summary>
        /// Intestazione ricevuta
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected string MakeHeader(Registro registro, ref int y)
        {
            var cmd = "";

            cmd += $"TEXT {h1} {x} 0 {registro.Header_1} \r\n";         // latte marche
            cmd += $"TEXT {p} {x} 50 {registro.Header_2} \r\n";         // Organizzazione Produttori            

            y = 150;

            return cmd;
        }

        /// <summary>
        /// Titolo
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected string MakeTitle(Registro registro, ref int y)
        {
            var cmd = "";

            cmd += $"TEXT {h1} {x} 100 {registro.Titolo} \r\n";         // Registro raccolta latte bovino o Registro consegna latte bovino

            return cmd;
        }

        /// <summary>
        /// Sezione Acquirente / Destinatario
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected string MakeAcquirenteDestinatarioSection(Registro registro, ref int y)
        {
            var cmd = "";

            int sxColWidth = WIDTH / 2;
            int lineSpacing = 30;

            // intestazione
            var headerAcq = PadRight("Acquirente", sxColWidth);
            var headerDest = PadRight("Destinatario", sxColWidth);
            y += 20;
            cmd += $"TEXT {p} {x} {y} {headerAcq} {headerDest}\r\n";

            var acq = registro.Acquirente;
            var dest = registro.Destinatario;

            // ragioni sociali
            var rsAcq = PadRight($"{acq?.RagioneSociale}", sxColWidth);
            var rsDest = PadRight($"{dest?.RagioneSociale}", sxColWidth);
            y += lineSpacing;
            cmd += $"TEXT {p} {x} {y} {rsAcq} {rsDest}\r\n";

            // indirizzi
            var indAcq = PadRight($"{acq?.Indirizzo}", sxColWidth);
            var indDest = PadRight($"{dest?.Indirizzo}", sxColWidth);
            y += lineSpacing;
            cmd += $"TEXT {p} {x} {y} {indAcq} {indDest}\r\n";

            // cap / comune / prov
            var comAcq = PadRight($"{acq?.CAP} {acq?.Comune} ({acq?.Provincia})", sxColWidth);
            var comDest = PadRight($"{dest?.CAP} {dest?.Comune} ({dest?.Provincia})", sxColWidth);
            y += lineSpacing;
            cmd += $"TEXT {p} {x} {y} {comAcq} {comDest}\r\n";

            // P IVA
            var pivaAcq = PadRight($"P.IVA {acq?.P_IVA}", sxColWidth);
            var pivaDest = PadRight($"P.IVA {dest?.P_IVA}", sxColWidth);
            y += lineSpacing;
            cmd += $"TEXT {p} {x} {y} {pivaAcq} {pivaDest}\r\n";

            y += lineSpacing;

            return cmd;
        }

        /// <summary>
        /// Sezione 
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected string MakeTrasportatoreSection(Registro registro, ref int y)
        {
            var cmd = "";

            int sxColWidth = WIDTH / 2;
            int lineSpacing = 30;
            var trasp = registro.Trasportatore;
            var automezzo = trasp != null ? trasp.AutoCisterna : new AutoCisterna();
            var cessionario = registro.Cessionario;

            // intestazione
            var headerTrasp = PadRight("Trasportatore", sxColWidth);
            var headerCess = PadRight(cessionario != null ? "1. Cessionario" : "", sxColWidth);            
            cmd += $"TEXT {p} {x} {y} {headerTrasp} {headerCess}\r\n";
            y += lineSpacing;

            // Targa - Rag. soc
            var targa = PadRight($"Targa automezzo: {automezzo?.Targa}", sxColWidth);
            var ragSoc = PadRight($"{cessionario?.RagioneSociale}", sxColWidth);

            cmd += $"TEXT {p} {x} {y} {targa} {ragSoc}\r\n";
            y += lineSpacing;

            // Trasportatore - Indirizzo
            var trasportatore = PadRight($"Trasportatore: {trasp?.RagioneSociale}", sxColWidth);
            var cessIndirizzo = PadRight($"{cessionario?.Indirizzo}", sxColWidth);
            cmd += $"TEXT {p} {x} {y} {trasportatore} {cessIndirizzo}\r\n";
            y += lineSpacing;

            // Indirizzo - cap/comune /prov
            var traspIndirizzo = PadRight($"{trasp?.Indirizzo}", sxColWidth);
            var provCess = !string.IsNullOrEmpty(cessionario?.Provincia) ? $"({ cessionario?.Provincia})" : "";
            var comCess = PadRight($"{cessionario?.CAP} {cessionario?.Comune} {provCess}", sxColWidth);
            cmd += $"TEXT {p} {x} {y} {traspIndirizzo} {comCess}\r\n";
            y += lineSpacing;

            // P IVA - P IVA
            var traspPiva = PadRight($"P.IVA: {trasp?.P_IVA}", sxColWidth);
            var cessPiva = PadRight(cessionario != null ? $"P.IVA: {cessionario?.P_IVA}" : "", sxColWidth);
            cmd += $"TEXT {p} {x} {y} {traspPiva} {cessPiva}\r\n";
            y += lineSpacing;

            return cmd;
        }

        /// <summary>
        /// Data e giro
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected string MakeDataSection(Registro registro, ref int y)
        {
            var cmd = "";

            int sxColWidth = 25;
            int lineSpacing = 50;
            var giro = registro.Giro;

            // Data / Giro
            var data = PadRight($"Data: {registro.Data.ToString("dd/MM/yyyy")}", sxColWidth);
            var giroText = PadRight($"Giro: {giro?.Descrizione}", sxColWidth);
            cmd += $"TEXT {h1} {x} {y} {data} {giroText}\r\n";
            y += lineSpacing;

            return cmd;
        }

        /// <summary>
        /// Sotto titolo
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected string MakeSubTitle(Registro registro, ref int y)
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

            cmd += $"TEXT {p} {x} {y} {registro.LatteCrudoConforme} \r\n";
            y += 1;
            cmd += $"TEXT {p} {x} {y} {registro.LatteCrudoConforme} \r\n";


            y += 25;
            y += 10;

            return cmd;
        }

        /// <summary>
        /// Sezione firme
        /// </summary>
        /// <param name="firmaSx"></param>
        /// <param name="firmaDx"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Linea spaziatrice
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        protected string MakeLine(ref int y)
        {
            var cmd = $"LINE 0 {y} 800 {y} 2 \r\n";
            y += 20;

            return cmd;
        }

        /// <summary>
        /// Divide una stringa in n-parti di lunghezza parti al chunkSize
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adatta la lunghezza della stringa in input (taglia se troppo lunga, aggiunge caratteri in fondo se troppo corta)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <param name="paddingChar"></param>
        /// <returns></returns>
        protected string PadRight(string source, int length, char paddingChar = ' ')
        {
            source = source.Replace("à", "a'");

            var result = String.Empty;

            if (source.Length >= length)
                result = source.Substring(0, length);

            if (source.Length < length)
                result = source.PadRight(length, ' ');

            return result;
        }

    }
}
