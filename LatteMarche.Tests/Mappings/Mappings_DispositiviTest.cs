using AutoMapper;
using FizzWare.NBuilder;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Application.Mobile.Dtos;
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
    public class Mappings_DispositiviTest
    {
        #region Constructors

        public Mappings_DispositiviTest()
        {
            AutoFacConfig.Configure();
            AutomapperConfig.Configure();
        }

        #endregion

        #region Methods

        [Test]
        public void DispositiviMappings_DispositivoMobile_To_DispositivoMobileDto()
        {
            var dispositivo = Builder<DispositivoMobile>
                .CreateNew()
                .Build();

            var dispositivoDto = Mapper.Map<DispositivoMobileDto>(dispositivo);

            Assert.AreEqual(dispositivoDto.Id, dispositivo.Id);

        }

        #endregion

    }
}
