using AutoMapper.Configuration;
using LatteMarche.Application.Documenti.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Documenti
{
    public class DocumentiMappings
    {
        public static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<Documento, DocumentoDto>();
            mappings.CreateMap<DocumentoDto, Documento>();

            return mappings;

        }
    }
}
