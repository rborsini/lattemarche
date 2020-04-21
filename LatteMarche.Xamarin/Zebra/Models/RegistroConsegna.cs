using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Models
{
    public class RegistroConsegna : Registro
    {
        public DateTime? DataPrelievo { get; set; }

        // Produttore
        public Allevamento Allevamento { get; set; }

        // Quantità
        public string Scomparto { get; set; }
        public decimal? Quantita_kg { get; set; }
        public decimal? Quantita_lt { get; set; }
        public decimal? Temperatura { get; set; }
        public int? NumeroMungiture { get; set; }
        public DateTime? DataUltimaMungitura { get; set; }
        public TipoLatte TipoLatte { get; set; }                

        // Quota latte
        public decimal? QuotaLatte_Qta_Tct { get; set; }
        public decimal? QuotaLatte_Prod_Rett { get; set; }
        public decimal? QuotaLatte_Qta_Res { get; set; }

        // Analisi qualità
        public decimal? AnalisiQualita_Grasso { get; set; }
        public decimal? AnalisiQualita_Proteine { get; set; }
        public decimal? AnalisiQualita_CBT_Ufc { get; set; }
        public decimal? AnalisiQualita_CS { get; set; }

        // Ultima Analisi
        public decimal? UltimaAnalisi_Media_Trim { get; set; }
        public decimal? UltimaAnalisi_Grasso { get; set; }
        public decimal? UltimaAnalisi_Proteine { get; set; }
        public decimal? UltimaAnalisi_CBT_Ufc { get; set; }
        public decimal? UltimaAnalisi_CS { get; set; }

        // Premi / penali
        public string PremiPenali { get; set; }

        // Informazioni
        public string Comunicazione { get; set; }

        public RegistroConsegna()
        {
            this.Titolo = "Registro consegna latte bovino";
        }
    }
}
