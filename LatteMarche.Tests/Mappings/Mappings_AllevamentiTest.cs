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
            var idTipoLatte = 2;

            var allevamento = Builder<Allevamento>
                .CreateNew()
                    .With(a => a.Utente = Builder<Utente>.CreateNew()
                            .With(u => u.IdTipoLatte = idTipoLatte)
                            .Build())
                .Build();

            var allevamentoDto = Mapper.Map<AllevamentoDto>(allevamento);

            Assert.AreEqual(allevamentoDto.CodiceAsl, allevamento.CodiceAsl);
            Assert.AreEqual(idTipoLatte, allevamentoDto.Utente_IdTipoLatte);

        }

        [Test]
        public void AllevamentiMappings_AllevamentoDto_To_Allevamento()
        {
            var idTipoLatte = 2;
            var allevamentoDto = Builder<AllevamentoDto>
                .CreateNew()
                    .With(a => a.Utente_IdTipoLatte = idTipoLatte)
                .Build();

            var allevamento = Mapper.Map<Allevamento>(allevamentoDto);

            Assert.AreEqual(allevamentoDto.CodiceAsl, allevamento.CodiceAsl);

            Assert.IsNotNull(allevamento.Utente);
            Assert.AreEqual(idTipoLatte, allevamento.Utente.IdTipoLatte);

        }

        #endregion

    }
}
