using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Utenti.Dtos
{
    public class UtenteDto : EntityDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }

    }

    public class UtentiMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<Utente, UtenteDto>();
            Mapper.CreateMap<UtenteDto, Utente>();
        }
    }

}
