using AutoMapper;
using FizzWare.NBuilder;
using LatteMarche.Application.Autocisterne.Dtos;
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
    public class Mappings_AutocisterneTest
    {

        #region Constructors

        public Mappings_AutocisterneTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();
        }

        #endregion

        #region Methods

        [Test]
        public void AutocisterneMappings_Autocisterna_To_AutocisternaDto()
        {
            var entity = Builder<Autocisterna>
                .CreateNew()
                .Build();

            var dto = Mapper.Map<AutocisternaDto>(entity);

            Assert.AreEqual(dto.Id, entity.Id);

        }

        [Test]
        public void AutocisterneMappings_AutocisternaDto_To_Autocisterna()
        {
            var dto = Builder<AutocisternaDto>
                .CreateNew()
                .Build();

            var entity = Mapper.Map<Autocisterna>(dto);

            Assert.AreEqual(entity.Id, dto.Id);

        }

        #endregion

    }
}
