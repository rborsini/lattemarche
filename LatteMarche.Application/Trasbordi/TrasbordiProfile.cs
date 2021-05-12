using AutoMapper;
using LatteMarche.Application.Trasbordi.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Trasbordi
{
    public class TrasbordiProfile : Profile
    {
        public TrasbordiProfile()
        {
            CreateMap<Trasbordo, TrasbordoDto>();
        }
    }
}
