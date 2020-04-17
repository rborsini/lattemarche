using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;

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
