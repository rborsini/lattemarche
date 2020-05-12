using LatteMarche.Xamarin.Zebra.Interfaces;
using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Makers.ZPL
{
    public abstract class AbstractLabelMaker : ILabelMaker
    {
        public int leftOffset = 50;
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

        protected string MakeLine(ref int y)
        {
            y += 220;
            var cmd = $"^FO{leftOffset},{y}^GB700,1,1^FS";
            return cmd;
        }

        protected string MakeAcquirenteSection(Registro registro, ref int y)
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
            cmd += $"^CFA,{h3}^FO{leftOffset},{y}^FD{registro.Acquirente.P_IVA}^FS"; // P.Iva

            return cmd;
        }       
        
        protected string MakeDestinatarioSection(Registro registro, ref int y)
        {
            var cmd = "";
            
            var leftOffsetDestinatario = 450; // Spazio sx della colonna Destinatario
            cmd += $"^CFA,{h3}^FO{leftOffsetDestinatario},240^FDDestinatario^FS"; // Stringa fissa Destinatario
            y -= 90;
            cmd += $"^CFA,{h3}^FO{leftOffsetDestinatario},{y}^FD{registro.Destinatario.RagioneSociale}^FS"; // Ragione sociale     
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffsetDestinatario},{y}^FD{registro.Destinatario.Indirizzo}^FS"; // Indirizzo
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffsetDestinatario},{y}^FD{registro.Destinatario.CAP} {registro.Destinatario.Comune} {registro.Destinatario.Provincia}^FS"; // CAP + Comune + Provincia
            y += 30;
            cmd += $"^CFA,{h3}^FO{leftOffsetDestinatario},{y}^FD{registro.Acquirente.P_IVA}^FS"; // P.Iva

            return cmd;
        }

    }
}
