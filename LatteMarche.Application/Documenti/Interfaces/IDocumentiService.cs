using System;
using LatteMarche.Application.Documenti.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using WeCode.Application.Interfaces;

namespace LatteMarche.Application.Documenti.Interfaces
{

    public interface IDocumentiService : IEntityService<Documento, int, DocumentoDto>
    {

    }

}
