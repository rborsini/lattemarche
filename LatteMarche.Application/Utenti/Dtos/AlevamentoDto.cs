using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Utenti.Dtos
{
    public class AllevamentoDto
    {
        public int Id { get; set; }
        public string CodiceAsl { get; set; }
        public string Indirizzo { get; set; }        
        public string SiglaProvincia { get; set; }
        public int IdComune { get; set; }
        public string CUAA { get; set; }
        public double? Latitudine { get; set; }
        public double? Longitudine { get; set; }

        public AllevatoreDto Allevatore { get; set; }

    }

}
