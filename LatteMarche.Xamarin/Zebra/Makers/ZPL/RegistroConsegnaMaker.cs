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

            // Dati latte prima sezione colonna SX
            cmd += MakeDatiLattePrimaSezioneSX(registroConsegna, 730);

            // Dati latte prima sezione colonna DX
            cmd += MakeDatiLattePrimaSezioneDX(registroConsegna, 730);

            // Linea
            cmd += MakeLine(830);

            // Dati latte seconda sezione colonna SX
            cmd += MakeDatiLatteSecondaSezioneSX(registroConsegna, 850);

            // Dati latte seconda sezione colonna DX
            cmd += MakeDatiLatteSecondaSezioneDX(registroConsegna, 850);

            // Linea
            cmd += MakeLine(1010);

            // Dati latte terza sezione colonna SX
            cmd += MakeDatiLatteTerzaSezioneSX(registroConsegna, 1030);

            // Dati latte terza sezione colonna DX
            cmd += MakeDatiLatteTerzaSezioneDX(registroConsegna, 1030);

            // Linea
            cmd += MakeLine(1190);

            // Informazioni
            cmd += MakeInformazioni(registroConsegna, 1210);

            // Linea
            cmd += MakeLine(1300);

            // Sezione firma produttore/delegato
            cmd += MakeFirmaProduttoreDelegato(1320);

            // Sezione firma trasportatore
            cmd += MakeFirmaTrasportatore(1320);

            // Chiudo il file con "^XZ"
            cmd += this.end_print;

            return cmd;
        }

    }
}
