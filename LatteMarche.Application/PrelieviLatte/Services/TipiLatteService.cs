using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Linq;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.PrelieviLatte.Services
{

    public class TipiLatteService: EntityService<TipoLatte, int, TipoLatteDto>, ITipiLatteService
    {

        #region Constructors

        public TipiLatteService(IUnitOfWork uow)
            : base(uow) { }

        #endregion

        #region Methods

        public DropDownDto DropDown()
        {
            var dropdown = new DropDownDto();

            dropdown.Items = this.repository.DbSet
                .Select(c => new DropDownItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Descrizione
                })
                .OrderBy(i => i.Text)
                .ToList();

            return dropdown;
        }

        protected override TipoLatte UpdateProperties(TipoLatte viewEntity, TipoLatte dbEntity)
        {
            dbEntity.Descrizione = viewEntity.Descrizione;
            dbEntity.DescrizioneBreve = viewEntity.DescrizioneBreve;
            dbEntity.FattoreConversione = viewEntity.FattoreConversione;
            dbEntity.FlagInvioSitra = viewEntity.FlagInvioSitra; 

            return dbEntity;
        }

        #endregion

    }

}
