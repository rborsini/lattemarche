using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Comuni.Dtos
{
    public class ComuneDto : EntityDto
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public string Provincia { get; set; }
        public string CAP { get; set; }
        
    }

}
