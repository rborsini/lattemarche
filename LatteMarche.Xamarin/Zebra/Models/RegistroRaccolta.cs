using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Zebra.Models
{
    public class RegistroRaccolta : Registro
    {
        public string CodiceLotto => $"{this.Giro.Codice}{DateTime.Now:ddMMyyyyHHmm}";

        public string Comunicazioni { get; set; }

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
            public decimal? Quantita_kg { get; set; }
        }

    }



}
