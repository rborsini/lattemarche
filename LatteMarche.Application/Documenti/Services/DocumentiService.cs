using System;
using LatteMarche.Application.Documenti.Dtos;
using LatteMarche.Application.Documenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;
using WeCode.Data.Interfaces;
using WeCode.Application;

namespace LatteMarche.Application.Documenti.Services
{

    public class DocumentiService : EntityService<Documento, int, DocumentoDto>, IDocumentiService
    {

        #region Constructors

        public DocumentiService(IUnitOfWork uow)
            : base(uow) { }

        protected override Documento UpdateProperties(Documento viewEntity, Documento dbEntity)
        {
            dbEntity.Descrizione = viewEntity.Descrizione;
            dbEntity.PathDocumento = viewEntity.PathDocumento;
            dbEntity.IdUtente = viewEntity.IdUtente;
            dbEntity.DataInserimento = viewEntity.DataInserimento;

            return dbEntity;
        }

        #endregion

    }

}