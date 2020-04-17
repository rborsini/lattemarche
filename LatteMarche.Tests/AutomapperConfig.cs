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

            LatteMarche.Application.AutomapperConfig.Configure();


        }
    }
}
