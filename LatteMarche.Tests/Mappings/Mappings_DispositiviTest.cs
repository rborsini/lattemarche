using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Core.Models;
using NUnit.Framework;

namespace LatteMarche.Tests.Mappings
{
    [TestFixture]
    public class Mappings_DispositiviTest
    {

        #region Fields

        private ILifetimeScope scope;
        private IMapper mapper;

        #endregion

        #region Constructors

        public Mappings_DispositiviTest()
        {
            AutoFacConfig.Configure();

            this.scope = AutoFacConfig.Container.BeginLifetimeScope();
            this.mapper = this.scope.Resolve<IMapper>();
        }

        #endregion

        #region Methods

        [Test]
        public void DispositiviMappings_DispositivoMobile_To_DispositivoMobileDto()
        {
            var entity = Builder<DispositivoMobile>
                .CreateNew()
                .Build();

            var dto = this.mapper.Map<DispositivoMobileDto>(entity);

            Assert.AreEqual(dto.Id, entity.Id);

        }

        #endregion

    }
}
