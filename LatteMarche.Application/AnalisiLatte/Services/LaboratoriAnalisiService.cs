using LatteMarche.Application.AnalisiLatte.Dtos;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Linq;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.AnalisiLatte.Services
{

    public class LaboratoriAnalisiService : EntityService<LaboratorioAnalisi, int, LaboratorioAnalisiDto>, ILaboratoriAnalisiService
    {

        #region Fields

        #endregion

        #region Constructors

        public LaboratoriAnalisiService(IUnitOfWork uow)
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
                .ToList();

            return dropdown;
        }

        protected override LaboratorioAnalisi UpdateProperties(LaboratorioAnalisi viewEntity, LaboratorioAnalisi dbEntity)
        {
            dbEntity.Descrizione = viewEntity.Descrizione;

            return dbEntity;
        }

        #endregion

    }

}
