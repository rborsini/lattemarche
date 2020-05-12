using LatteMarche.Xamarin.Zebra.Makers.CPCL;
using LatteMarche.Xamarin.Zebra.Models;
using System;

namespace ZebraPrinterlConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var registroConsegna = new RegistroConsegna()
            {
                Acquirente = new LatteMarche.Xamarin.Db.Models.Acquirente()
                {
                    CAP = "60024",
                    Comune = "Filottrano",
                    Indirizzo = "Via Montepulciano",
                    Provincia = "AN",
                    P_IVA = "1232135",
                    RagioneSociale = "SIBILLA SOC. COOP. AGR."
                },
                Destinatario = new LatteMarche.Xamarin.Db.Models.Destinatario()
                {
                    CAP = "60024",
                    Comune = "Filottrano",
                    Indirizzo = "Via Montepulciano",
                    Provincia = "AN",
                    P_IVA = "1232135",
                    RagioneSociale = "SIBILLA SOC. COOP. AGR."
                },
                DataPrelievo = DateTime.Today
            };

            var cpclMaker = new LatteMarche.Xamarin.Zebra.Makers.CPCL.RegistroConsegnaMaker();
            var zpllMaker = new LatteMarche.Xamarin.Zebra.Makers.ZPL.RegistroConsegnaMaker();

            var cpcl = cpclMaker.MakeLabel(registroConsegna);
            var zpl = zpllMaker.MakeLabel(registroConsegna);

        }
    }
}
