using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;

namespace LatteMarche.Tests
{
    public static class AutomapperConfig
    {
        public static void Configure()
        {

            var mappings = new MapperConfigurationExpression();

            mappings = LatteMarche.Application.AutomapperConfig.Configure(mappings);
            mappings = LatteMarche.Application.Mobile.AutomapperConfig.Configure(mappings);

            Mapper.Reset();
            Mapper.Initialize(mappings);


        }
    }
}
