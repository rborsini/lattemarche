using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Acquirenti.Dtos
{
    public class AcquirenteDto
    {
        public int Id { get; set; }
        public string RagioneSociale { get; set; }
        public string Piva { get; set; }
        public string Indirizzo { get; set; }
        public string SiglaProvincia { get; set; }
        public int IdComune { get; set; }
        public int? IdSitra { get; set; }
    }

}
