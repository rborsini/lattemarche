using AutoMapper;
using FizzWare.NBuilder;
using LatteMarche.Application.Acquirenti.Dtos;
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
    public class Mappings_AcquirentiTest
    {

        #region Constructors

        public Mappings_AcquirentiTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();
        }

        #endregion

        #region Methods

        [Test]
        public void AcquirentiMappings_Acquirente_To_AcquirenteDto()
        {
            var entity = Builder<Acquirente>
                .CreateNew()
                    .With(a => a.Comune = Builder<Comune>.CreateNew().Build())
                .Build();

            var dto = Mapper.Map<AcquirenteDto>(entity);

            Assert.AreEqual(dto.Id, entity.Id);
            Assert.AreEqual(dto.SiglaProvincia, entity.Comune.Provincia);

        }

        [Test]
        public void AcquirentiMappings_AcquirenteDto_To_Acquirente()
        {
            var dto = Builder<AcquirenteDto>
                .CreateNew()
                .Build();

            var entity = Mapper.Map<Acquirente>(dto);

            Assert.AreEqual(entity.Id, dto.Id);

        }

        #endregion

    }
}
