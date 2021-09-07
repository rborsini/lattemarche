using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Models
{
    public class RegistroRaccolta : Registro
    {
        public string CodiceLotto { get; set; }

        public string Giro_Text => $"Giro: {this.CodiceLotto}";
        public string Lotto_Text => $"Lotto: {this.Giro.Descrizione}";

        public string Comunicazioni { get; set; }

        public bool LavaggioCisterna { get; set; }

        public string LavaggioCisterna_Text => this.LavaggioCisterna ? "Effettuato lavaggio cistena" : "";

        public List<Item> Items { get; set; }

        public RegistroRaccolta()
        {
            this.Items = new List<Item>();
            this.Titolo = "Registro raccolta latte bovino";
            this.Comunicazioni = "Latte crudo destinato alla produzione di latte fresco pastorizzato di Alta Qualita' in possesso dei requisiti di composizione igienico-sanitari previsti dal D.M. 185/91";
        }

        public class Item
        {
            public string Scomparto { get; set; }
            public Allevamento Allevamento { get; set; }
            public TipoLatte TipoLatte { get; set; }
            public DateTime? DataPrelievo { get; set; }

            public string DataPrelievo_Text => this.DataPrelievo.HasValue ? $"{this.DataPrelievo.Value:HH:mm\ndd/MM/yyyy}" : "";

            public decimal? Quantita_kg { get; set; }

            public decimal? Quantita_lt => this.Quantita_kg.HasValue && this.TipoLatte != null && this.TipoLatte.FattoreConversione.HasValue ? this.Quantita_kg.Value / this.TipoLatte.FattoreConversione.Value : (decimal?)null;

            public string Quantita_kg_Text => this.Quantita_kg.HasValue ? $"{Convert.ToInt32(this.Quantita_kg.Value)}" : string.Empty;
            public string Quantita_lt_Text => this.Quantita_lt.HasValue ? $"{Convert.ToInt32(this.Quantita_lt.Value)}" : string.Empty;


            public string Trasbordo { get; set; }
        }

    }



}
