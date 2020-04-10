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
            var registroConsegna = registro as RegistroConsegna;

            int y = 0;
            var cmd = "";

            // intestazione
            cmd += MakeHeader(registroConsegna, ref y);

            // titolo
            cmd += MakeTitle(registroConsegna, ref y);

            // sotto titolo
            cmd += MakeSubTitle(registroConsegna, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Acquirente / Destinatario
            cmd += MakeAcquirenteDestinatarioSection(registroConsegna, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // Automezzo
            cmd += MakeTrasportatoreSection(registroConsegna, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // data e giro
            cmd += MakeDataSection(registroConsegna, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // produttore
            cmd += MakeProduttoreSection(registroConsegna, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // dettaglio prelievo
            cmd += MakerDettaglioPrelievoSection(registroConsegna, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // quota latte
            cmd += MakerQuotaLatteSection(registroConsegna, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // ultima analisi
            cmd += MakerUltimaAnalisiSection(registroConsegna, ref y);

            // linea separatrice
            cmd += MakeLine(ref y);

            // informazioni
            cmd += MakeInformazioniSection(registroConsegna, ref y);

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

        /// <summary>
        /// Sezione Produttore
        /// </summary>
        /// <param name="registroConsegna"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string MakeProduttoreSection(RegistroConsegna registroConsegna, ref int y)
        {
            var cmd = "";

            int lineSpacing = 30;

            // Produttore:
            cmd += $"TEXT {p} {x} {y} Produttore: \r\n";
            y += lineSpacing;

            var prod = registroConsegna.Prelievo.Allevamento;
            var text = PadRight($"{prod.RagioneSociale} {prod.CAP} {prod.Prov} - {prod.Comune} - {prod.P_IVA}", WIDTH);

            cmd += $"TEXT {p} {x} {y} {text} \r\n";
            y += (lineSpacing * 2);

            return cmd;
        }
    
        /// <summary>
        /// Tabella di riepilogo del dettaglio del prelievo
        /// </summary>
        /// <param name="registroConsegna"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string MakerDettaglioPrelievoSection(RegistroConsegna registroConsegna, ref int y)
        {
            var cmd = "";

            int lineSpacing = 30;
            int colSxWidth = 25;
            int colDxStart = (WIDTH / 2);

            // Qta kg   -   Litri
            var qtaKg = $"{registroConsegna.Prelievo.Quantita_kg.Value:.0}";
            var qtaLt = $"{registroConsegna.Prelievo.Quantita_lt.Value:.0}";

            cmd += $"TEXT {p} {x} {y} {PadRight("Quantita' kg: ", colSxWidth)}{PadRight(qtaKg, colDxStart - colSxWidth)} Litri: {qtaLt} \r\n";
            y += lineSpacing;

            // Num Munte   -   Ora
            var numMunte = $"{registroConsegna.Prelievo.NumeroMungiture}";
            var ora = $"{registroConsegna.Prelievo.DataUltimaMungitura.Value:HH:mm}";

            cmd += $"TEXT {p} {x} {y} {PadRight("N. Munte: ", colSxWidth)}{PadRight(numMunte, colDxStart - colSxWidth)} Ora: {ora} \r\n";
            y += lineSpacing;

            // Temp   -   Tipo Latte
            var temp = $"{registroConsegna.Prelievo.Temperatura.Value:.0}";
            var tipoLatte = $"{registroConsegna.Prelievo.TipoLatte.Codice} - {registroConsegna.Prelievo.TipoLatte.Descrizione}";

            cmd += $"TEXT {p} {x} {y} {PadRight("Temp. : ", colSxWidth)}{PadRight(temp, colDxStart - colSxWidth)} Tipo Latte: {tipoLatte} \r\n";

            y += (lineSpacing * 2);

            return cmd;
        }

        /// <summary>
        /// Sezione quota latte
        /// </summary>
        /// <param name="registroConsegna"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string MakerQuotaLatteSection(RegistroConsegna registroConsegna, ref int y)
        {
            var cmd = "";

            int lineSpacing = 30;
            int colSxWidth = 25;
            int colDxStart = (WIDTH / 2);

            // Quota latte      Analisi qualità:
            cmd += $"TEXT {p} {x} {y} {PadRight("Quota latte", WIDTH - colDxStart)}Analisi Qualita': \r\n";
            y += lineSpacing;

            // Quantità Tct.    Grasso %
            var tct = $"kg 671059";
            var grasso = $"p/v";

            cmd += $"TEXT {p} {x} {y} {PadRight("Quantita' Tct.: ", colSxWidth)}{PadRight(tct, colDxStart - colSxWidth)} Grasso % {grasso} \r\n";
            y += lineSpacing;

            // Prod. Rett. /%Gr)    Proteine % p/v
            var prodRett = $"kg 0";
            var proteine = $"p/v";

            cmd += $"TEXT {p} {x} {y} {PadRight("Prod.Rett. (%Gr): ", colSxWidth)}{PadRight(prodRett, colDxStart - colSxWidth)} Proteine % {proteine} \r\n";
            y += lineSpacing;

            //// Quota res.:          C.B.T. ufc/ml
            var quotaRes = $"kg 671059";
            var cbt = $"";

            cmd += $"TEXT {p} {x} {y} {PadRight("Quota res.: ", colSxWidth)}{PadRight(quotaRes, colDxStart - colSxWidth)} C.B.T. ufc/ml % {cbt} \r\n";
            y += lineSpacing;

            ////                      C.S./ml:
            var cs = $"";

            cmd += $"TEXT {p} {x} {y} {PadRight("", colDxStart)}C.S./ml: {cs} \r\n";


            y += (lineSpacing * 2);

            return cmd;
        }

        /// <summary>
        /// Sezione ultima analisi
        /// </summary>
        /// <param name="registroConsegna"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string MakerUltimaAnalisiSection(RegistroConsegna registroConsegna, ref int y)
        {
            var cmd = "";

            int lineSpacing = 30;
            int colSxWidth = 25;
            int colDxStart = (WIDTH / 2);

            // Media Trim.      Premi/Penal.
            var mediaTrim = $"3,70";
            var premi = $"Euro/1000lt";

            cmd += $"TEXT {p} {x} {y} {PadRight("Media Trim. ", colSxWidth)}{PadRight(mediaTrim, colDxStart - colSxWidth)} Premi/Penal. {premi} \r\n";
            y += lineSpacing;

            // Grasso
            var grasso = $"p/v";

            cmd += $"TEXT {p} {x} {y} {PadRight("Grasso % ", colSxWidth)}{PadRight(grasso, colDxStart - colSxWidth)} \r\n";
            y += lineSpacing;

            // Proteine
            var proteine = $"p/v";

            cmd += $"TEXT {p} {x} {y} {PadRight("Proteine % ", colSxWidth)}{PadRight(proteine, colDxStart - colSxWidth)} \r\n";
            y += lineSpacing;

            // C.B.T.
            var cbt = $"";

            cmd += $"TEXT {p} {x} {y} {PadRight("C.B.T. ufc/ml ", colSxWidth)}{PadRight(cbt, colDxStart - colSxWidth)} \r\n";
            y += lineSpacing;

            // C.S.
            var cs = $"";

            cmd += $"TEXT {p} {x} {y} {PadRight("C.S./ml ", colSxWidth)}{PadRight(cs, colDxStart - colSxWidth)} \r\n";


            y += (lineSpacing * 2);

            return cmd;
        }

        /// <summary>
        /// Sezione Informazioni
        /// </summary>
        /// <param name="registroConsegna"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string MakeInformazioniSection(RegistroConsegna registroConsegna, ref int y)
        {
            var cmd = "";

            int lineSpacing = 30;

            // Informazioni:
            cmd += $"TEXT {p} {x} {y} Informazioni: \r\n";
            y += lineSpacing;

            foreach(var chunk in SplitInLine(registroConsegna.Comunicazione, WIDTH))
            {
                cmd += $"TEXT {p} {x} {y} {chunk} \r\n";
                y += lineSpacing;
            }

            y += lineSpacing ;

            return cmd;
        }


    }
}
