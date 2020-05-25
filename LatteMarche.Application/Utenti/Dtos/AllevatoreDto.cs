using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Utenti.Dtos
{
    public class AllevatoreDto : UtenteDto
    {

        public int IdTipoLatte { get; set; }
        public string CodiceAllevatore { get; set; }

        public List<AllevamentoDto> Allevamenti { get; set; }

        public AllevatoreDto()
        {
            this.Allevamenti = new List<AllevamentoDto>();
        }

    }
}
