using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Models
{
    public class RegistroConsegna : Registro
    {
        public AutoCisterna AutoCisterna;

        public DateTime? DataPrelievo { get; set; }

        // Produttore
        public Allevamento Allevamento { get; set; }

        public string Produttore_Text => $"{this.Allevamento?.RagioneSociale} {this.Allevamento?.CAP} {this.Allevamento?.Provincia} - {this.Allevamento?.Comune} - {this.Allevamento?.P_IVA}";

        // Quantità
        public string Scomparto { get; set; }
        public decimal? Quantita_kg { get; set; }
        public decimal? Quantita_lt { get; set; }
        public decimal? Temperatura { get; set; }
        public int? NumeroMungiture { get; set; }
        public DateTime? DataUltimaMungitura { get; set; }
        public TipoLatte TipoLatte { get; set; }

        public string Quantita_kg_Text => this.Quantita_kg.HasValue ? $"Quantità kg: {Convert.ToInt32(this.Quantita_kg.Value)}" : string.Empty;
        public string Quantita_lt_Text => this.Quantita_lt.HasValue ? $"Quantità lt: {Convert.ToInt32(this.Quantita_lt.Value)}" : string.Empty;
        public string NumeroMungiture_Text => this.NumeroMungiture.HasValue ? $"N. Mung.: {this.NumeroMungiture.Value}" : string.Empty;
        public string Temperatura_Text => this.Temperatura.HasValue ? $"Temp. : {this.Temperatura.Value:#.0}" : string.Empty;
        public string TipoLatte_Text => this.TipoLatte != null ? $"Tipo Latte: {this.TipoLatte.Codice}" : string.Empty;

        // Quota latte
        public decimal? QuotaLatte_Qta_Tct { get; set; }
        public decimal? QuotaLatte_Prod_Rett { get; set; }
        public decimal? QuotaLatte_Qta_Res { get; set; }
        public decimal? Progressivo_Conferimenti { get; set; }
        public decimal? Mensile { get; set; }
        public decimal? Annuale { get; set; }

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

        public string Giro_Text => this.Giro != null ? this.Giro.Descrizione : string.Empty;

        public RegistroConsegna()
        {
            this.Titolo = "Registro consegna latte bovino";
        }
    }
}
