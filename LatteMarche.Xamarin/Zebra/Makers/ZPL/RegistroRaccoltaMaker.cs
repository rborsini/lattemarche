using LatteMarche.Xamarin.Zebra.Models;
using System;
using System.Collections.Generic;
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

    }
}
