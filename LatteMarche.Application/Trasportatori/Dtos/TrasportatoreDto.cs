using System.Collections.Generic;

namespace LatteMarche.Application.Trasportatori.Dtos
{
    public class TrasportatoreDto : EntityDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }

        public string NomeCompleto => $"{this.Cognome} {this.Nome}";

        public string Indirizzo { get; set; }
        public string Telefono { get; set; }
        public string Cellulare { get; set; }
        public string Comune { get; set; }       
        public string Provincia { get; set; }
        public string P_IVA { get; set; }
        public string RagioneSociale { get; set; }

        public List<GiroDto> Giri { get; set; }

    }



}
