using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Makers.ZPL
{
    public class RegistroRaccoltaMaker : AbstractLabelMaker
    {
        public override string MakeLabel(Registro registro)
        {
            var registroRaccolta = registro as RegistroRaccolta;

            var cmd = "";

            // Apro il file con "^XA"
            cmd += this.start_print;

            // Header
            cmd += MakeHeader();

            // Header
            cmd += MakeHeader();

            // Linea
            cmd += MakeLine(220);

            // Sezione acquirente/destinatario
            cmd += MakeAcquirenteDestinatarioSection(registroRaccolta, 210);

            // Linea
            cmd += MakeLine(410);

            // Dati trasportatore
            cmd += MakeDatiTrasportatore(registroRaccolta, 430);

            // Linea
            cmd += MakeLine(560);

            // Dati giro
            cmd += MakeDatiGiro(registroRaccolta, 580);

            // Linea
            cmd += MakeLine(650);

            // Sezione scomparti
            cmd += MakeScompartiSection(registroRaccolta, 690);

            // Linea
            cmd += MakeLine(890);

            // Tabella
            cmd += MakeTabellaSection(registroRaccolta, 910);

            // Chiudo il file con "^XZ"
            cmd += this.end_print;

            return cmd;
        }

        // Sezione dati giro
        private string MakeDatiGiro(RegistroRaccolta registro, int y)
        {
            var cmd = "";

            // Colonna SX
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDData: {registro.Data}^FS"; // Data
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDOra: {registro.Data}^FS"; // Ora

            y -= 30;
            // Colonna DX
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDGiro: {registro.Giro}^FS"; // Giro
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDLotto: {registro.CodiceLotto}^FS"; // Lotto

            return cmd;
        }

        // Sezione scomparti
        private string MakeScompartiSection(RegistroRaccolta registro, int y)
        {
            var cmd = "";

            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDLatte bovino crudo convenzionale scomparto N°: 1 2 3 4 ^FS";
            y += 60;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDLatte crudo destinato alla produzione di latte fresco^FS";
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDpastorizzato di Alta Qualità in possesso dei requisiti^FS";
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDdi composizione igienico-sanitari previsti dal DM 185/91^FS";
            y += 30;
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDscomparto N°: 1 2 3 4^FS";

            return cmd;
        }

        private string MakeTabellaSection(RegistroRaccolta registro, int y)
        {
            var cmd = "";
            var tableFontSize = 15;

            // HEADER
            cmd += $"^CFA,{tableFontSize}^FO{leftOffset},{y}^FDSCOM^FS";
            y += 20;
            cmd += $"^CFA,{tableFontSize}^FO{leftOffset},{y}^FDPARTO^FS"; // Scomparto

            y -= 20;
            cmd += $"^CFA,{tableFontSize}^FO110,{y}^FDPRODUTTORE^FS";
            y += 20;
            cmd += $"^CFA,{tableFontSize}^FO110,{y}^FDP.IVA-PROV.^FS"; // Produttore/P.IVA-Prov.

            y -= 20;
            cmd += $"^CFA,{tableFontSize}^FO350,{y}^FDTIPO^FS"; // Tipo
            cmd += $"^CFA,{tableFontSize}^FO440,{y}^FDKG^FS"; // Kg
            cmd += $"^CFA,{tableFontSize}^FO490,{y}^FDORA^FS"; // Ora

            cmd += $"^CFA,{tableFontSize}^FO580,{y}^FDFirma^FS";
            y += 20;
            cmd += $"^CFA,{tableFontSize}^FO580,{y}^FDProd/Del^FS"; // Firma prod/del

            y -= 20;
            cmd += $"^CFA,{tableFontSize}^FO710,{y}^FDFirma^FS";
            y += 20;
            cmd += $"^CFA,{tableFontSize}^FO710,{y}^FDConduc.^FS"; // Firma conducente

            y += 40;
            cmd += $"^FO{leftOffset},{y}^GB{lineWidth},1,1^FS"; // Linea

            // CONTENUTO TABELLA
            decimal quantitaTotale = 0;
            foreach (var prelievo in registro.Items.OrderBy(p => p.Scomparto))
            {

                y += 20;
                cmd += $"^CFA,{tableFontSize}^FO{leftOffset},{y}^FD{prelievo.Scomparto}^FS"; // Numero scomparto
                cmd += $"^CFA,{tableFontSize}^FO110,{y}^FD{prelievo.Allevamento}^FS"; // Nome produttore
                y += 20;
                cmd += $"^CFA,{tableFontSize}^FO110,{y}^FD{prelievo.Allevamento.P_IVA}-{prelievo.Allevamento.Provincia}^FS"; // P.iva / Prov.
                y -= 20;
                cmd += $"^CFA,{tableFontSize}^FO350,{y}^FD{prelievo.TipoLatte}^FS"; // Tipo
                cmd += $"^CFA,{tableFontSize}^FO440,{y}^FD{prelievo.Quantita_kg}^FS"; // Kg
                cmd += $"^CFA,{tableFontSize}^FO490,{y}^FD{prelievo.DataPrelievo}^FS"; // Ora
                y += 20;
                cmd += $"^CFA,{tableFontSize}^FO490,{y}^FD03/02/2020^FS"; // Data
                y += 40;
                cmd += $"^FO{leftOffset},{y}^GB{lineWidth},1,1^FS"; // Linea

                quantitaTotale += prelievo.Quantita_kg.HasValue ? prelievo.Quantita_kg.Value : 0;
            }

            // TOTALI
            y += 20;
            cmd += $"^CFA,{tableFontSize}^FO110,{y}^FDTOTALI^FS";
            cmd += $"^CFA,{tableFontSize}^FO440,{y}^FD{quantitaTotale.ToString("#0.00")}^FS";
            y += 40;
            cmd += $"^FO{leftOffset},{y}^GB{lineWidth},1,1^FS"; // Linea

            y += 40;

            // FIRME
            // Colonna SX
            cmd += $"^CFA,{h2}^FO{leftOffset},{y}^FDFirma Acquirente/Delegato ^FS"; // Firma acquirente/delegato
            y += 100;
            cmd += $"^FO{leftOffset},{y}^GB250,1,1^FS";

            // Colonna DX
            y -= 100;
            cmd += $"^CFA,{h2}^FO{leftOffsetColonnaDX},{y}^FDFirma Destinatario ^FS"; // Firma destinatario
            y += 100;
            cmd += $"^FO{leftOffsetColonnaDX},{y}^GB250,1,1^FS";

            return cmd;
        }

    }
}
