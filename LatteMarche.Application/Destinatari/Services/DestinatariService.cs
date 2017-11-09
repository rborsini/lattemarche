using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Application.Destinatari.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Destinatari.Services
{
    public class DestinatariService : EntityReadOnlyService<Destinatario, int, DestinatarioDto>, IDestinatariService
    {
        #region Fields

        #endregion

        #region Constructors

        public DestinatariService(IUnitOfWork uow)
            : base(uow) { }

        #endregion

        #region Methods

        #endregion
    }
}
