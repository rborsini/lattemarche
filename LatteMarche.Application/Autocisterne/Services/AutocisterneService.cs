using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Autocisterne.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Linq;
using WeCode.Application;
using WeCode.Data.Interfaces;
using AutoMapper;

namespace LatteMarche.Application.Autocisterne.Interfaces
{
    public class AutocisterneService : EntityService<Autocisterna, int, AutocisternaDto>, IAutocisterneService
    {
        #region Fields        

        #endregion

        #region Constructors

        public AutocisterneService(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        #endregion

        #region Methods

        public DropDownDto DropDown(int idAutotrasportatore)
        {
            DropDownDto model = new DropDownDto();

            var query = this.repository.Query;

            model.Items = query
                .Where(a => a.IdTrasportatore == idAutotrasportatore)
                .Select(item => new DropDownItem() { Value = item.Id.ToString(), Text = item.Targa })
                .Distinct()
                .OrderBy(i => i.Text)
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
