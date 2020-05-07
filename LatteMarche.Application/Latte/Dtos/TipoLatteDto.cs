using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Latte.Dtos
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
