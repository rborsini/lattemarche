using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.CPCL
{
    public class RegistroRaccoltaMaker : AbstractLabelMaker
    {


        public override string MakeLabel(Registro registro)
        {
            var registroRaccolta = registro as RegistroRaccolta;

            int y = 0;
            var cmd = "";

            // intestazione
            cmd += MakeHeader(registroRaccolta, ref y);

            // titolo
            cmd += MakerTitle(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Acquirente / Destinatario
            cmd += MakeAcquirenteDestinatarioSection(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Automezzo
            cmd += MakeTrasportatoreSection(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // lotto 
            cmd += MakeLottoSection(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // scomparto
            cmd += MakeScompartoSection(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // tabella scomparti
            cmd += MakeTabellaSection(registroRaccolta, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Firme
            cmd += MakeFirmeSection("Firma Acquirente \\ Delegato", "Firma Destinatario", ref y);

            // intestazione label aggiunta alla fine per avere l'altezza corretta
            var height = y + 100;
            cmd = $"! {offset} 200 200 {height} {quantity}\r\n" + cmd;
            //cmd += "FORM\r\n";
            cmd += "PRINT\r\n";

            return cmd;

        }

        private string MakeAcquirenteDestinatarioSection(Registro registro, ref int y)
        {
            var cmd = "";

            int sxColWidth = WIDTH / 2;
            int lineSpacing = 30;

            // intestazione
            var headerAcq = PadRight("Acquirente", sxColWidth);
            var headerDest = PadRight("Destinatario", sxColWidth);
            y += 20;
            cmd += $"TEXT {p} {x} {y} {headerAcq} {headerDest}\r\n";

            // ragioni sociali
            var rsAcq = PadRight(registro.Acquirente.RagioneSociale, sxColWidth);
            var rsDest = PadRight(registro.Destinatario.RagioneSociale, sxColWidth);
            y += lineSpacing;
            cmd += $"TEXT {p} {x} {y} {rsAcq} {rsDest}\r\n";

            // indirizzi
            var indAcq = PadRight(registro.Acquirente.Indirizzo, sxColWidth);
            var indDest = PadRight(registro.Destinatario.Indirizzo, sxColWidth);
            y += lineSpacing;
            cmd += $"TEXT {p} {x} {y} {indAcq} {indDest}\r\n";

            // cap / comune / prov
            var comAcq = PadRight($"{registro.Acquirente.CAP} {registro.Acquirente.Comune} ({registro.Acquirente.Provincia})", sxColWidth);
            var comDest = PadRight($"{registro.Destinatario.CAP} {registro.Destinatario.Comune} ({registro.Destinatario.Provincia})", sxColWidth);
            y += lineSpacing;
            cmd += $"TEXT {p} {x} {y} {comAcq} {comDest}\r\n";

            // P IVA
            var pivaAcq = PadRight($"P.IVA {registro.Acquirente.P_IVA}", sxColWidth);
            var pivaDest = PadRight($"P.IVA {registro.Destinatario.P_IVA}", sxColWidth);
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
            var data = PadRight($"Data: {registro.Data.ToString("dd/MM/yyyy")}", sxColWidth);
            var giro = PadRight($"Giro: {registro.Giro.Nome}", sxColWidth);
            cmd += $"TEXT {h1} {x} {y} {data} {giro}\r\n";
            y += lineSpacing;

            // Ora / Lotto
            var ora = PadRight($"Ora: {registro.Data.ToString("HH:mm")}", sxColWidth);
            var lotto = PadRight($"Lotto: {registro.Lotto.Codice}", sxColWidth);
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

            foreach (var riga in SplitInLine(registro.Comunicazioni, WIDTH))
            {
                cmd += $"TEXT {p} {x} {y} {riga} \r\n";
                y += lineSpacing;
            }

            cmd += $"TEXT {p} {x} {y} scomparto N. 1 2 3 4 \r\n";
            y += (lineSpacing * 2);

            return cmd;
        }

        private string MakeTabellaSection(RegistroRaccolta registro, ref int y)
        {
            var cmd = "";

            // intestazione
            y += 30;
            cmd += $"TEXT {p} {x} {y} {PadRight("SCOMP", 7, ' ')} {PadRight("PRODUTTORE", 30, ' ')} {PadRight("TIPO", 10, ' ')} {PadRight("kg", 5, ' ')} {PadRight("ORA", 11, ' ')} {PadRight("Firma", 16, ' ')} {PadRight("Firma", 16, ' ')} \r\n";
            y += 25;
            cmd += $"TEXT {p} {x} {y} {PadRight("PARTO", 7, ' ')} {PadRight("P.IVA-PROV.", 30, ' ')} {PadRight("", 10, ' ')} {PadRight("", 5, ' ')} {PadRight("", 11, ' ')} {PadRight("Prod\\Del", 16, ' ')} {PadRight("Conducente", 16, ' ')} \r\n";

            decimal qtaTot = 0;
            foreach (var prelievo in registro.Lotto.Prelievi.OrderBy(p => p.Scomparto))
            {
                y += 40;

                var scomparto = prelievo.Scomparto;
                var ragioneSociale = prelievo.Allevamento.RagioneSociale;
                var pIvaProv = $"{prelievo.Allevamento.P_IVA}-{prelievo.Allevamento.Prov}";
                var tipo = prelievo.TipoLatte.Descrizione;
                var qta = prelievo.Quantita.ToString();
                var ora = prelievo.DataPrelievo.Value.ToString("HH:mm");
                var data = prelievo.DataPrelievo.Value.ToString("dd/MM/yyyy");

                qtaTot += prelievo.Quantita.HasValue ? prelievo.Quantita.Value : 0;

                cmd += $"TEXT {p} {x} {y} {PadRight(scomparto, 7, ' ')} {PadRight(ragioneSociale, 28, ' ')}   {PadRight(tipo, 10, ' ')} {PadRight(qta, 5, ' ')} {PadRight(ora, 11, ' ')} {PadRight("", 16, ' ')} {PadRight("", 16, ' ')} \r\n";

                y += 25;
                cmd += $"TEXT {p} {x} {y} {PadRight("", 7, ' ')} {PadRight(pIvaProv, 30, ' ')} {PadRight("", 10, ' ')} {PadRight("", 5, ' ')} {PadRight(data, 11, ' ')} {PadRight("", 16, ' ')} {PadRight("", 16, ' ')} \r\n";
            }

            // Totali
            y += 40;
            cmd += $"TEXT {p} {x} {y} {PadRight("", 7, ' ')} {PadRight("TOTALI", 30, ' ')} {PadRight("", 10, ' ')} {PadRight(qtaTot.ToString("#0.00"), 5, ' ')} {PadRight("", 11, ' ')} {PadRight("", 16, ' ')} {PadRight("", 16, ' ')} \r\n";

            y += 40;

            return cmd;
        }






    }
}
