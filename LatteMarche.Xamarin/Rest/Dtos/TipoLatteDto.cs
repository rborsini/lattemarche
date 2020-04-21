using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Xamarin.Rest.Dtos
{
    public class TipoLatteDto
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public string DescrizioneBreve { get; set; }
        public decimal FattoreConversione { get; set; }
        public bool FlagInvioSitra { get; set; }
    }
}
