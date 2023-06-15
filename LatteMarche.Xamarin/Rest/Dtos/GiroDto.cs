using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Rest.Dtos
{
    public class GiroDto 
    {
        public int Id { get; set; }        
        public string Codice { get; set; }
        public string Descrizione { get; set; }
        public int IdTrasportatore { get; set; }

        public List<AllevamentoDto> Allevamenti { get; set; }

        public GiroDto()
        {
            this.Allevamenti = new List<AllevamentoDto>();
        }

    }
}
