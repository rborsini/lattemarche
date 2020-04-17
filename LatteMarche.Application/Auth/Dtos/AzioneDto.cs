using AutoMapper;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Auth.Dtos
{
    public class AzioneDto : EntityDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ViewItem { get; set; }
        public string Pagina { get; set; }
        public string Nome { get; set; }
    }

}
