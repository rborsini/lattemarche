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

            //int y = 0;
            var cmd = "";

            // Apro il file con "^XA"
            cmd += this.start_print;

            // Header
            cmd += MakeHeader();

            // Linea
            cmd += MakeLine(220);

            // Sezione acquirente/destinatario
            cmd += MakeAcquirenteDestinatarioSection(registroConsegna, 210);
            
            // Linea
            cmd += MakeLine(410);

            // Dati trasportatore
            cmd += MakeDatiTrasportatore(registroConsegna, 430);

            // Linea
            cmd += MakeLine(560);

            // Dati giro
            cmd += MakeDatiGiro(registroConsegna, 580);

            // Linea
            cmd += MakeLine(620);

            // Dati produttore
            cmd += MakeDatiProduttore(registroConsegna, 640);

            // Linea
            cmd += MakeLine(710);

            // Dati latte prima sezione
            cmd += MakeDatiLatteFirstSection(registroConsegna, 730);

            // Linea
            cmd += MakeLine(830);

            // Dati latte seconda sezione
            cmd += MakeDatiLatteSecondSection(registroConsegna, 850);

            // Linea
            cmd += MakeLine(1010);

            // Dati latte terza sezione
            cmd += MakeDatiLatteThirdSection(registroConsegna, 1030);

            // Linea
            cmd += MakeLine(1190);

            // Informazioni
            cmd += MakeInformazioni(registroConsegna, 1210);

            // Linea
            cmd += MakeLine(1300);

            // Sezione firme
            cmd += MakeFirmeSection(registro, 1320);

            // Chiudo il file con "^XZ"
            cmd += this.end_print;

            return cmd;
        }

        // Sezione dati giro
        private string MakeDatiGiro(RegistroConsegna registroConsegna, int y)
        {
            var cmd = "";

            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDData: {registroConsegna.Data}^FS"; // Data
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDGiro: {registroConsegna.Giro}^FS"; // Giro

            return cmd;
        }

        // Sezione dati produttore
        private string MakeDatiProduttore(RegistroConsegna registroConsegna, int y)
        {
            var cmd = "";

            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDProduttore:^FS"; // Produttore
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FD{registroConsegna.Allevamento}^FS"; // Nome produttore

            return cmd;
        }

        // Sezione dati latte prima sezione
        private string MakeDatiLatteFirstSection(RegistroConsegna registroConsegna, int y)
        {
            var cmd = "";

            // Colonna SX
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDQuantita' Kg: {registroConsegna.Quantita_kg}^FS"; // Quantità
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDN. Munte: {registroConsegna.NumeroMungiture}^FS"; // Numero munte
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDN. Temp. °C: {registroConsegna.Temperatura}^FS"; // Temperatura

            // Colonna DX
            y -= 60;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDLitri: {registroConsegna.Quantita_lt}^FS"; // Litri
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDOra: {registroConsegna.DataPrelievo}^FS"; // Ora
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDTipo latte: {registroConsegna.TipoLatte}^FS"; // Tipo latte

            return cmd;
        }

        // Sezione dati latte seconda sezione
        private string MakeDatiLatteSecondSection(RegistroConsegna registroConsegna, int y)
        {
            var cmd = "";

            // Colonna SX
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDQuota latte: {registroConsegna.QuotaLatte_Prod_Rett}^FS"; // Quota latte
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDQuantità Tct: {registroConsegna.QuotaLatte_Qta_Tct}^FS"; // Quantità
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDProd. Rett. (%Gr): {registroConsegna.QuotaLatte_Qta_Res}^FS"; // Prod. Rett.
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDQuota Res.: {registroConsegna.QuotaLatte_Qta_Res}^FS"; // Quota Res

            // Colonna DX
            y -= 90;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDAnalisi qualita': {registroConsegna.AnalisiQualita_CBT_Ufc}^FS"; // Analisi qualità
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDGrasso % p/v: {registroConsegna.AnalisiQualita_Grasso}^FS"; // Grasso
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDProteine % p/v: {registroConsegna.AnalisiQualita_Proteine}^FS"; // Proteine
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDC.B.T. ufc/ml: {registroConsegna.AnalisiQualita_CBT_Ufc}^FS"; // C.B.T.
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDC.S./ml: {registroConsegna.AnalisiQualita_CS}^FS"; // C.S.

            return cmd;
        }

        // Sezione dati latte terza sezione
        private string MakeDatiLatteThirdSection(RegistroConsegna registroConsegna, int y)
        {
            var cmd = "";

            // Colonna SX
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDMedia trim: {registroConsegna.UltimaAnalisi_Media_Trim}^FS"; // Media Trim.
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDGrasso % p/v: {registroConsegna.UltimaAnalisi_Grasso}^FS"; // Grasso
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDProteine % p/v: {registroConsegna.UltimaAnalisi_Proteine}^FS"; // Proteine
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDC.B.T. ufc/ml: {registroConsegna.UltimaAnalisi_CBT_Ufc}^FS"; // C.B.T.
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDC.S./ml: ^FS"; // C.S.

            // Colonna DX
            y -= 120;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDPremi/Penal. €/1000lt: {registroConsegna.PremiPenali}^FS"; // Premi/Penali

            return cmd;
        }

        // Sezione informazioni
        private string MakeInformazioni(RegistroConsegna registroConsegna, int y)
        {
            var cmd = "";

            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDInformazioni: ^FS"; // Informazioni
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FD{registroConsegna.Comunicazione}^FS"; // Contenuto delle informazioni

            return cmd;
        }

    }
}
