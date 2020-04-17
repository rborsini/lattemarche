using AutoMapper;
using FizzWare.NBuilder;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Tests.Mappings
{
    [TestFixture]
    public class Mappings_AllevamentiTest
    {

        #region Constructors

        public Mappings_AllevamentiTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();
        }

        #endregion

        #region Methods

        [Test]
        public void AllevamentiMappings_Allevamento_To_AllevamentoDto()
        {
            Assert.IsTrue(true);

            var allevamento = Builder<Allevamento>
                .CreateNew()
                .Build();

            var allevamentoDto = Mapper.Map<AllevamentoDto>(allevamento);

            Assert.AreEqual(allevamentoDto.CodiceAsl, allevamento.CodiceAsl);

        }

        [Test]
        public void AllevamentiMappings_AllevamentoDto_To_Allevamento()
        {
            var allevamentoDto = Builder<AllevamentoDto>
                .CreateNew()
                .Build();

            var allevamento = Mapper.Map<Allevamento>(allevamentoDto);

            Assert.AreEqual(allevamentoDto.CodiceAsl, allevamento.CodiceAsl);

        }

        #endregion

    }
}
