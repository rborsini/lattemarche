using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Documenti.Dtos
{
    public class DocumentoDto
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        public string PathDocumento { get; set; }
        public int IdUtente { get; set; }
        public DateTime? DataInserimento { get; set; }
    }

}
