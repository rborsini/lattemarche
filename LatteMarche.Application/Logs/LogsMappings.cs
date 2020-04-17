using AutoMapper.Configuration;
using LatteMarche.Application.Logs.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Logs
{
    public class LogsMappings
    {
        public static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<LogRecord, LogRecordDto>();

            mappings.CreateMap<LogRecordDto, LogRecord>();

            return mappings;
        }
    }
}
