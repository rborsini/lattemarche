using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Makers.CPCL
{
    public class RegistroRaccoltaMaker : AbstractLabelMaker
    {

        public override string MakeLabel(Registro registro)
        {
            var registroRaccolta = registro as RegistroRaccolta;

            int y = 0;
            var cmd = "";

            //// intestazione
            //cmd += MakeHeader(registroRaccolta, ref y);

            // titolo
            cmd += MakeTitle(registroRaccolta, ref y);

            // sotto titolo
            cmd += MakeSubTitle(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Acquirente / Destinatario
            cmd += MakeAcquirenteDestinatarioSection(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Automezzo
            cmd += MakeTrasportatoreSection(registroRaccolta, ref y);

            // Lavaggio cisterna
            cmd += MakeLavaggioCisternaSection(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // data e giro
            cmd += MakeDataSection(registroRaccolta, ref y);

            // lotto 
            cmd += MakeLottoSection(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            //// scomparto
            //cmd += MakeScompartoSection(registroRaccolta, ref y);

            //// linea separatrice
            //cmd += MakeLine(ref y);

            // tabella scomparti
            cmd += MakeTabellaSection(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Firme
            cmd += MakeFirmeSection("Firma Acquirente \\ Delegato", "Firma Destinatario", ref y);

            // Footer
            cmd += MakeFooter(registro, ref y);

            // intestazione label aggiunta alla fine per avere l'altezza corretta
            var height = y + 100;
            cmd = $"! {offset} 200 200 {height} {registro.NumeroCopie}\r\n" + cmd;
            //cmd += "FORM\r\n";
            cmd += "PRINT\r\n";

            return cmd;

        }

        /// <summary>
        /// Sezione lavaggio cisterna
        /// </summary>
        /// <param name="registroRaccolta"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string MakeLavaggioCisternaSection(RegistroRaccolta registroRaccolta, ref int y)
        {
            int lineSpacing = 30;
            var cmd = "";

            if(registroRaccolta.LavaggioCisterna)
            {
                y += lineSpacing;
                cmd += $"TEXT {p} {x} {y} Effettuato lavaggio cistena \r\n";
                y += lineSpacing * 2;
            }          

            return cmd;
        }

        /// <summary>
        /// Sezione lotto
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string MakeLottoSection(RegistroRaccolta registro, ref int y)
        {
            var cmd = "";

            int sxColWidth = 25;
            int lineSpacing = 50;

            // Ora / Lotto
            var ora = PadRight($"Ora: {registro.Data.ToString("HH:mm")}", sxColWidth);
            var lotto = PadRight($"Lotto: {registro.CodiceLotto}", sxColWidth);
            cmd += $"TEXT {h1} {x} {y} {ora} {lotto}\r\n";
            y += lineSpacing + (lineSpacing / 2);       // interlinea 1.5 

            return cmd;
        }

        /// <summary>
        /// Sezione scomparto
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string MakeScompartoSection(RegistroRaccolta registro, ref int y)
        {
            var cmd = "";

            int lineSpacing = 30;

            // Latte bovino
            cmd += $"TEXT {p} {x} {y} Latte bovino crudo convenzionale scomparto N. 1 2 3 4 \r\n";
            y += (lineSpacing * 2);

            foreach (var riga in SplitInLine(registro.Comunicazioni, WIDTH))
            {
                cmd += $"TEXT {p} {x} {y} {riga} \r\n";
                y += lineSpacing;
            }

            cmd += $"TEXT {p} {x} {y} scomparto N. 1 2 3 4 \r\n";
            y += (lineSpacing * 2);

            return cmd;
        }

        /// <summary>
        /// Tabella produttori
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string MakeTabellaSection(RegistroRaccolta registro, ref int y)
        {
            var cmd = "";

            // intestazione
            y += 30;
            cmd += $"TEXT {p} {x} {y} {PadRight("SCOMP", 7, ' ')} {PadRight("PRODUTTORE", 30, ' ')} {PadRight("TIPO", 10, ' ')} {PadRight("kg", 5, ' ')} {PadRight("ORA", 11, ' ')} {PadRight("Firma", 16, ' ')} {PadRight("Firma", 16, ' ')} \r\n";
            y += 25;
            cmd += $"TEXT {p} {x} {y} {PadRight("PARTO", 7, ' ')} {PadRight("P.IVA-PROV.", 30, ' ')} {PadRight("", 10, ' ')} {PadRight("", 5, ' ')} {PadRight("", 11, ' ')} {PadRight("Prod\\Del", 16, ' ')} {PadRight("Conducente", 16, ' ')} \r\n";

            decimal qtaTot = 0;
            foreach (var prelievo in registro.Items.OrderBy(p => p.Scomparto))
            {
                y += 40;

                var allevamento = prelievo.Allevamento;
                var tipoLatte = prelievo.TipoLatte;

                var scomparto = $"{prelievo.Scomparto}";
                var ragioneSociale = $"{allevamento?.RagioneSociale}";
                var pIvaProv = $"{allevamento?.P_IVA}-{allevamento?.Provincia}";
                var tipo = $"{tipoLatte?.Codice}";
                var qta = $"{prelievo.Quantita_kg:#}";
                var ora = $"{prelievo.DataPrelievo:HH:mm}";
                var data = $"{prelievo.DataPrelievo:dd/MM/yyyy}";

                qtaTot += prelievo.Quantita_kg.HasValue ? prelievo.Quantita_kg.Value : 0;

                var aa = PadRight(scomparto, 7, ' ');

                cmd += $"TEXT {p} {x} {y} {PadRight(scomparto, 7, ' ')} {PadRight(ragioneSociale, 28, ' ')}   {PadRight(tipo, 10, ' ')} {PadRight(qta, 5, ' ')} {PadRight(ora, 11, ' ')} {PadRight("", 16, ' ')} {PadRight("", 16, ' ')} \r\n";

                y += 25;
                cmd += $"TEXT {p} {x} {y} {PadRight("", 7, ' ')} {PadRight(pIvaProv, 30, ' ')} {PadRight("", 10, ' ')} {PadRight("", 5, ' ')} {PadRight(data, 11, ' ')} {PadRight("", 16, ' ')} {PadRight("", 16, ' ')} \r\n";
            }

            // Totali
            y += 40;
            cmd += $"TEXT {p} {x} {y} {PadRight("", 7, ' ')} {PadRight("TOTALI", 30, ' ')} {PadRight("", 10, ' ')} {PadRight(qtaTot.ToString("#"), 5, ' ')} {PadRight("", 11, ' ')} {PadRight("", 16, ' ')} {PadRight("", 16, ' ')} \r\n";

            y += 40;

            return cmd;
        }

    }
}
