using System;
using AutoMapper;
using LatteMarche.Core.Models;
using System.Collections.Generic;

namespace LatteMarche.Application.Trasportatori.Dtos
{
    public class GiroDto 
    {
        public int Id { get; set; }
        public string Denominazione { get; set; }
        public string CodiceGiro { get; set; }
        public int IdTrasportatore { get; set; }

        public List<GiroItemDto> Items { get; set; }

        public GiroDto()
        {
            this.Items = new List<GiroItemDto>();
        }

    }

}
