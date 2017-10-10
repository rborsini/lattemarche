using System;

using LatteMarche.Application.LaboratoriAnalisi.Dtos;
using LatteMarche.Application.LaboratoriAnalisi.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.LaboratoriAnalisi.Services
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
