using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Allevamenti.Dtos
{
    public class AllevatoreDto : EntityDto
    {
        public int Id { get; set; }
        public string CodiceAsl { get; set; }
        public string IndirizzoAllevamento { get; set; }
        public string Comune { get; set; }
        public string Provincia { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string RagioneSociale { get; set; }
        public int IdUtente { get; set; }
        public int IdComune { get; set; }
        public int? IdSitraStabilimentoAllevamento { get; set; }
        public int? IdTipoLatte { get; set; }

    }


}
