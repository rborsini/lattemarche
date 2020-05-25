using AutoMapper;
using FizzWare.NBuilder;
using LatteMarche.Application.AziendeTrasportatori.Dtos;
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
    public class Mappings_AziendeTrasportatoriTest
    {

        #region Constructors

        public Mappings_AziendeTrasportatoriTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();
        }

        #endregion

        #region Methods

        [Test]
        public void AziendeTrasportatoriMappings_AziendaTrasportatori_To_AziendaTrasportatoriDto()
        {
            var entity = Builder<AziendaTrasportatori>
                .CreateNew()
                .Build();

            var dto = Mapper.Map<AziendaTrasportatoriDto>(entity);

            Assert.AreEqual(dto.Id, entity.Id);

        }

        [Test]
        public void AziendeTrasportatoriMappings_AziendaTrasportatoriDto_To_AziendaTrasportatori()
        {
            var dto = Builder<AziendaTrasportatoriDto>
                .CreateNew()
                .Build();

            var entity = Mapper.Map<AziendaTrasportatori>(dto);

            Assert.AreEqual(entity.Id, dto.Id);

        }

        #endregion

    }
}
