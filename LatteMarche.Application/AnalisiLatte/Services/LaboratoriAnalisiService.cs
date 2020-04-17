using LatteMarche.Application.AnalisiLatte.Dtos;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;



namespace LatteMarche.Application.AnalisiLatte.Services
{

    public class LaboratoriAnalisiService : EntityReadOnlyService<LaboratorioAnalisi, int, LaboratorioAnalisiDto>, ILaboratoriAnalisiService
    {

        #region Fields

        #endregion

        #region Constructors

        public LaboratoriAnalisiService(IUnitOfWork uow)
            : base(uow) { }

        #endregion

        #region Methods

        #endregion

    }

}
