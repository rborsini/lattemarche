using AutoMapper;
using FizzWare.NBuilder;
using LatteMarche.Application.Cessionari.Dtos;
using LatteMarche.Application.Utenti.Dtos;
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
    public class Mappings_CessionariTest
    {

        #region Constructors

        public Mappings_CessionariTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();
        }

        #endregion

        #region Methods

        [Test]
        public void CessionariMappings_Cessionario_To_CessionarioDto()
        {
            var entity = Builder<Cessionario>
                .CreateNew()
                    .With(a => a.Comune = Builder<Comune>.CreateNew().Build())
                .Build();

            var dto = Mapper.Map<CessionarioDto>(entity);

            Assert.AreEqual(dto.Id, entity.Id);
            Assert.AreEqual(dto.SiglaProvincia, entity.Comune.Provincia);

        }

        [Test]
        public void CessionariMappings_Cessionario_To_Cessionario()
        {
            var dto = Builder<CessionarioDto>
                .CreateNew()
                .Build();

            var entity = Mapper.Map<Cessionario>(dto);

            Assert.AreEqual(entity.Id, dto.Id);

        }

        #endregion

    }
}

