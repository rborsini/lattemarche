using AutoMapper;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Trasportatori.Dtos
{
    public class AutocisternaDto : EntityDto
    {
        public int Id { get; set; }

        public string Marca { get; set; }
        public string Modello { get; set; }
        public string Targa { get; set; }
        
        public int? IdTrasportatore { get; set; }
        public int? Portata { get; set; }
        public int? NumScomparti { get; set; }

    }


}
