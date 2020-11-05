using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using LatteMarche.Application.Destinatari.Dtos;
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
    public class Mappings_DestinatariTest
    {
        #region Fields

        private ILifetimeScope scope;
        private IMapper mapper;

        #endregion

        #region Constructors

        public Mappings_DestinatariTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();
            this.mapper = this.scope.Resolve<IMapper>();
        }

        #endregion

        #region Methods

        [Test]
        public void DestinatariMappings_Destinatario_To_DestinatarioDto()
        {
            var entity = Builder<Destinatario>
                .CreateNew()
                    .With(a => a.Comune = Builder<Comune>.CreateNew().Build())
                .Build();

            var dto = this.mapper.Map<DestinatarioDto>(entity);

            Assert.AreEqual(dto.Id, entity.Id);
            Assert.AreEqual(dto.SiglaProvincia, entity.Comune.Provincia);

        }

        [Test]
        public void DestinatariMappings_DestinatarioDto_To_Destinatario()
        {
            var dto = Builder<DestinatarioDto>
                .CreateNew()
                .Build();

            var entity = this.mapper.Map<Destinatario>(dto);

            Assert.AreEqual(entity.Id, dto.Id);

        }

        #endregion

    }
}