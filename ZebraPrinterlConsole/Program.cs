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
                    RagioneSociale = "SIBILLA SOC. COOP. AGRICOLANIONIO NIOJMIO MIOPèMO"
                },
                Destinatario = new LatteMarche.Xamarin.Db.Models.Destinatario()
                {
                    CAP = "60024",
                    Comune = "Filottrano",
                    Indirizzo = "Via Montepulciano molto molto molto ma moltissismissimo lungo",
                    Provincia = "AN",
                    P_IVA = "1232135",
                    RagioneSociale = "SIBILLA SOC. COOP. AGR."
                },
                Trasportatore = new LatteMarche.Xamarin.Db.Models.Trasportatore()
                {
                    RagioneSociale = "SIBILLA SOC. COOP. AGR.",
                    Indirizzo = "Via Montepulciano",
                    P_IVA = "1232135",
                    //AutoCisterna = new LatteMarche.Xamarin.Db.Models.AutoCisterna()
                    //{
                    //    Targa = "12RTY56",
                    //},
                },
                Giro = new LatteMarche.Xamarin.Db.Models.TemplateGiro()
                {
                    Descrizione = "PESARO-ANCONA"
                },
                Allevamento = new LatteMarche.Xamarin.Db.Models.Allevamento()
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

            var registroRaccolta = new RegistroRaccolta()
            {
                Acquirente = new LatteMarche.Xamarin.Db.Models.Acquirente()
                {
                    CAP = "60024",
                    Comune = "Filottrano",
                    Indirizzo = "Via Montepulciano molto molto molto ma moltissismissimo lungo",
                    Provincia = "AN",
                    P_IVA = "1232135",
                    RagioneSociale = "SIBILLA SOC. COOP. AGRICOLANIONIO NIOJMIO MIOPèMO."
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
                Trasportatore = new LatteMarche.Xamarin.Db.Models.Trasportatore()
                {
                    RagioneSociale = "SIBILLA SOC. COOP. AGR.",
                    Indirizzo = "Via Montepulciano",
                    P_IVA = "1232135",
                    AutoCisterna = new LatteMarche.Xamarin.Db.Models.AutoCisterna()
                    {
                        Targa = "12RTY56",
                    },
                },
                Giro = new LatteMarche.Xamarin.Db.Models.TemplateGiro()
                {
                    Descrizione = "PESARO-ANCONA"
                },
                CodiceLotto = "P10302200920",
                Items = new System.Collections.Generic.List<RegistroRaccolta.Item>()
                {
                    new RegistroRaccolta.Item()
                    {
                        Scomparto = "1",
                        Allevamento = new LatteMarche.Xamarin.Db.Models.Allevamento() { RagioneSociale = "TRIONFI HONORATI S.R.L." },
                        DataPrelievo = DateTime.Now,
                        Quantita_kg = Convert.ToDecimal(1.324),
                        TipoLatte = new LatteMarche.Xamarin.Db.Models.TipoLatte(){ Codice = "QM-AQ"}
                    },
                    new RegistroRaccolta.Item()
                    {
                        Scomparto = "2",
                        Allevamento = new LatteMarche.Xamarin.Db.Models.Allevamento() { RagioneSociale = "TRIONFI HONORATI S.R.L. ARCADIA SPA" },
                        DataPrelievo = DateTime.Now,
                        Quantita_kg = Convert.ToDecimal(1.324),
                        TipoLatte = new LatteMarche.Xamarin.Db.Models.TipoLatte(){ Codice = "QM-AQ"}
                    }
                }
            };

            //var cpclMaker = new LatteMarche.Xamarin.Zebra.Makers.CPCL.RegistroConsegnaMaker();
            var zplMaker = new LatteMarche.Xamarin.Zebra.Makers.ZPL.RegistroConsegnaMaker();
            var zplMakerRegRaccolta = new LatteMarche.Xamarin.Zebra.Makers.ZPL.RegistroRaccoltaMaker();

            //var cpcl = cpclMaker.MakeLabel(registroConsegna);
            var zplRegConsegna = zplMaker.MakeLabel(registroConsegna);
            var zplRegRaccolta = zplMakerRegRaccolta.MakeLabel(registroRaccolta);

        }
    }
}
