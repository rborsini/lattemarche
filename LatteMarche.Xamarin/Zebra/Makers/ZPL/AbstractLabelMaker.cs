using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Makers.ZPL
{
    public abstract class AbstractLabelMaker : ILabelMaker
    {
        public int leftOffset = 50;
        public int leftOffsetColonnaDX = 450;
        public int h2 = 22;
        public int h3 = 14;
        public int h6 = 12;
        public string start_print = "^XA";
        public string end_print = "^XZ";

        public abstract string MakeLabel(Registro registro);

        protected string MakeHeader()
        {
            var header_line_1 = $"^CFA,{h2}^FO{leftOffset},50^FDLatte Marche^FS";
            var header_line_2 = $"^CFA,{h6}^FO{leftOffset},80^FDOrganizzazione Produttori^FS";
            var header_line_3 = $"^CFA,{h2}^FO{leftOffset},120^FDRegistro consegna latte bovino^FS";
            var header_line_4 = $"^CFA,{h6}^FO{leftOffset},160^FDL. 119/03-D.M. 31/07/03, art.12 - Documentazione raccolta latte - Sistema Informatizzato di registrazione - ^FS";
            var header_line_5 = $"^CFA,{h6}^FO{leftOffset},180^FDAutorizzazione Regione Marche DDS 512/SAR^FS";

            return header_line_1 + header_line_2 + header_line_3 + header_line_4 + header_line_5;
        }        

        protected string MakeLine(int y)
        {
            var cmd = $"^FO{leftOffset},{y}^GB700,1,1^FS";
            return cmd;
        }

        protected string MakeAcquirenteSection(Registro registro, int y)
        {
            var cmd = "";

            // Blocco sx colonna Acquirente
            cmd += $"^CFA,{h3}^FO{leftOffset},240^FDAcquirente^FS"; // Stringa fissa Acquirente
            y += 60;
            cmd += $"^CFA,{h3}^FO{leftOffset},{y}^FD{registro.Acquirente.RagioneSociale}^FS"; // Ragione sociale       
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffset},{y}^FD{registro.Acquirente.Indirizzo}^FS"; // Indirizzo
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffset},{y}^FD{registro.Acquirente.CAP} {registro.Acquirente.Comune} {registro.Acquirente.Provincia}^FS"; // CAP + Comune + Provincia
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffset},{y}^FDP.Iva: {registro.Acquirente.P_IVA}^FS"; // P.Iva

            return cmd;
        }       
        
        protected string MakeDestinatarioSection(Registro registro, int y)
        {
            var cmd = "";
            
            cmd += $"^CFA,{h3}^FO{leftOffsetColonnaDX},240^FDDestinatario^FS"; // Stringa fissa Destinatario
            y += 60;
            cmd += $"^CFA,{h3}^FO{leftOffsetColonnaDX},{y}^FD{registro.Destinatario.RagioneSociale}^FS"; // Ragione sociale     
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffsetColonnaDX},{y}^FD{registro.Destinatario.Indirizzo}^FS"; // Indirizzo
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffsetColonnaDX},{y}^FD{registro.Destinatario.CAP} {registro.Destinatario.Comune} {registro.Destinatario.Provincia}^FS"; // CAP + Comune + Provincia
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffsetColonnaDX},{y}^FDP.IVA: {registro.Acquirente.P_IVA}^FS"; // P.Iva

            return cmd;
        }

        protected string MakeDatiTrasportatore(Registro registro, int y)
        {
            var cmd = "";

            cmd += $"^CFA,{h3}^FO{leftOffset},{y}^FDTarga automezzo: {registro.Trasportatore.AutoCisterna.Targa}^FS"; // Targa automezzo
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffset},{y}^FDTrasportatore: {registro.Trasportatore.RagioneSociale}^FS"; // Ragione sociale
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffset},{y}^FD{registro.Trasportatore.Indirizzo}^FS"; // Indirizzo
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffset},{y}^FDP.IVA: {registro.Trasportatore.P_IVA}^FS"; // Indirizzo

            return cmd;
        }    
        
        protected string MakeDatiGiro(Registro registro, int y)
        {
            var cmd = "";

            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDData: {registro.Data}^FS"; // Data
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDGiro: {registro.Giro}^FS"; // Giro

            return cmd;
        }

        protected string MakeDatiProduttore(Registro registro, int y)
        {
            var cmd = "";

            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDProduttore:^FS"; // Produttore
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDdjadkjahkdjhakdhaskjdhaksjdhkjashkjd^FS"; // Nome produttore

            return cmd;
        }

        protected string MakeDatiLattePrimaSezioneSX(Registro registro, int y)
        {
            var cmd = "";

            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDQuantita': ^FS"; // Quantità
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDN. Munte: ^FS"; // Numero munte
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDN. Temp. °C: ^FS"; // Temperatura

            return cmd;
        }

        protected string MakeDatiLattePrimaSezioneDX(Registro registro, int y)
        {
            var cmd = "";

            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDLitri: ^FS"; // Litri
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDOra: ^FS"; // Ora
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDTipo latte: ^FS"; // Tipo latte

            return cmd;
        }

        protected string MakeDatiLatteSecondaSezioneSX(Registro registro, int y)
        {
            var cmd = "";

            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDQuota latte: ^FS"; // Quota latte
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDQuantita': ^FS"; // Quantità
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDProd. Rett. (%Gr): ^FS"; // Prod. Rett.
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDQuota Res.: ^FS"; // Quota Res

            return cmd;
        }

        protected string MakeDatiLatteSecondaSezioneDX(Registro registro, int y)
        {
            var cmd = "";

            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDAnalisi qualita': ^FS"; // Analisi qualità
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDGrasso % p/v: ^FS"; // Grasso
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDProteine % p/v: ^FS"; // Proteine
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDC.B.T. ufc/ml: ^FS"; // C.B.T.
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDC.S./ml: ^FS"; // C.S.

            return cmd;
        }

    }
}
