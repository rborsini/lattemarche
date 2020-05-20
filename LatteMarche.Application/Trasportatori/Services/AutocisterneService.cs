using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Linq;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Trasportatori.Interfaces
{
    public class AutocisterneService : EntityService<Autocisterna, int, AutocisternaDto>, IAutocisterneService
    {
        #region Fields        

        #endregion

        #region Constructors

        public AutocisterneService(IUnitOfWork uow)
            : base(uow)
        {
        }

        #endregion

        #region Methods

        public DropDownDto DropDown(int? idTrasportatore)
        {
            DropDownDto model = new DropDownDto();

            var query = this.repository.Query;

            if (idTrasportatore.HasValue)
                query = query.Where(c => c.IdTrasportatore == idTrasportatore);

            model.Items = query
                .Select(item => new DropDownItem() { Value = item.Id.ToString(), Text = item.Targa })
                .Distinct()
                .ToList();

            return model;
        }

        protected override Autocisterna UpdateProperties(Autocisterna viewEntity, Autocisterna dbEntity)
        {
            dbEntity.IdTrasportatore = viewEntity.IdTrasportatore;
            dbEntity.Marca = viewEntity.Marca;
            dbEntity.Modello = viewEntity.Modello;
            dbEntity.NumScomparti = viewEntity.NumScomparti;
            dbEntity.Portata = viewEntity.Portata;
            dbEntity.Targa = viewEntity.Targa;

            return dbEntity;
        }

        #endregion
    }
}
