using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Application.Dtos;

namespace LatteMarche.Application.Utenti.Dtos
{
    public class UtentiSearchDto : BaseSearchDto
    {
        public int? IdProfilo { get; set; }

        public override bool IsEmpty => this.IsFilterModelEmpty<UtentiSearchDto>(this);

        public string RagioneSociale { get; set; }

        public string Nome { get; set; }

        public string Cognome { get; set; }

        public string Username { get; set; }

        public string FullText { get; set; }
        public string Tenant { get; set; }
    }
}
