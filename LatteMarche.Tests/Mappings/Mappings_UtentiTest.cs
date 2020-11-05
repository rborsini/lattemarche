using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using LatteMarche.Application.Allevamenti.Dtos;
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
    public class Mappings_UtentiTest
    {

        #region Fields

        private ILifetimeScope scope;
        private IMapper mapper;

        #endregion

        #region Constructor

        public Mappings_UtentiTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();
            this.mapper = this.scope.Resolve<IMapper>();
        }

        #endregion

        #region Methods

        [Test]
        public void UtentiMappings_Utente_To_UtenteDto()
        {
            var entity = Builder<Utente>
                .CreateNew()
                    .With(a => a.Comune = Builder<Comune>.CreateNew().Build())
                    .With(a => a.UtenteXAcquirente = Builder<UtenteXAcquirente>.CreateNew().Build())
                    .With(a => a.Allevamenti = Builder<Allevamento>.CreateListOfSize(2).Build().ToList())
                .Build();

            var dto = this.mapper.Map<UtenteDto>(entity);
            Assert.AreEqual(dto.Id, entity.Id);

            Assert.AreEqual(dto.IdAcquirente, entity.UtenteXAcquirente.IdAcquirente);
            Assert.AreEqual(2, dto.Allevamenti.Count);
        }

        [Test]
        public void UtentiMappings_UtenteDto_To_Utente()
        {
            var dto = Builder<UtenteDto>
                .CreateNew()
                    .With(a => a.Allevamenti = Builder<AllevamentoDto>.CreateListOfSize(2).Build().ToList())
                .Build();

            var entity = this.mapper.Map<Utente>(dto);
            Assert.AreEqual(entity.Id, dto.Id);

            Assert.IsNotNull(entity.UtenteXAcquirente);
            Assert.AreEqual(entity.UtenteXAcquirente.IdAcquirente, dto.IdAcquirente.Value);
            Assert.AreEqual(2, entity.Allevamenti.Count);
        }

        #endregion

    }
}
