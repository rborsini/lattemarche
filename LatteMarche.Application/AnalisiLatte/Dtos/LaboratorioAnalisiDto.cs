using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.AnalisiLatte.Dtos
{
    public class LaboratorioAnalisiDto : EntityDto
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        
    }


}
