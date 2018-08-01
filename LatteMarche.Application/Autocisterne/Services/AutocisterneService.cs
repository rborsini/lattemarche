using LatteMarche.Application.Autocisterne.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Autocisterne.Interfaces
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
