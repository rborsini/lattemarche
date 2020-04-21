using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Dtos
{
    public class TemplateGiroDto
    {
        public int Id { get; set; }
        public string Denominazione { get; set; }
        public string CodiceGiro { get; set; }

        public List<AllevamentoDto> Allevamenti { get; set; }

        public TemplateGiroDto()
        {
            this.Allevamenti = new List<AllevamentoDto>();
        }

    }
}
