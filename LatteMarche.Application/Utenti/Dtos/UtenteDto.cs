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

        public string PivaCF { get; set; }
        public string Indirizzo { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int IdProfilo { get; set; }
        public bool? Abilitato { get; set; }
        public bool? Visibile { get; set; }
        public string RagioneSociale { get; set; }
        public string CodiceAllevatore { get; set; }
        public int QuantitaLatte { get; set; }

      /* campi da aggiungere
      ,[TELEFONO]
      ,[CELLULARE]
      ,[ID_COMUNE]
      ,[SESSO]
      ,[ID_TIPO_LATTE]
      ,[NUMERO_COMUNICAZIONE]
      ,[NOTE]*/
    }

    public class UtentiMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<Utente, UtenteDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                .ForMember(dest => dest.Cognome, opts => opts.MapFrom(src => src.Cognome.Trim()))
                .ForMember(dest => dest.PivaCF, opts => opts.MapFrom(src => src.PivaCF.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.Login, opts => opts.MapFrom(src => src.Login.Trim()))
                .ForMember(dest => dest.Password, opts => opts.MapFrom(src => src.Password.Trim()))
                .ForMember(dest => dest.IdProfilo, opts => opts.MapFrom(src => src.IdProfilo))
                .ForMember(dest => dest.Abilitato, opts => opts.MapFrom(src => src.Abilitato))
                .ForMember(dest => dest.Visibile, opts => opts.MapFrom(src => src.Visibile))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.CodiceAllevatore, opts => opts.MapFrom(src => src.CodiceAllevatore.Trim()))
                .ForMember(dest => dest.QuantitaLatte, opts => opts.MapFrom(src => src.QuantitaLatte))
                ;
            Mapper.CreateMap<UtenteDto, Utente>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                .ForMember(dest => dest.Cognome, opts => opts.MapFrom(src => src.Cognome.Trim()))
                .ForMember(dest => dest.PivaCF, opts => opts.MapFrom(src => src.PivaCF.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.Login, opts => opts.MapFrom(src => src.Login.Trim()))
                .ForMember(dest => dest.Password, opts => opts.MapFrom(src => src.Password.Trim()))
                .ForMember(dest => dest.IdProfilo, opts => opts.MapFrom(src => src.IdProfilo))
                .ForMember(dest => dest.Abilitato, opts => opts.MapFrom(src => src.Abilitato))
                .ForMember(dest => dest.Visibile, opts => opts.MapFrom(src => src.Visibile))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.CodiceAllevatore, opts => opts.MapFrom(src => src.CodiceAllevatore.Trim()))
                .ForMember(dest => dest.QuantitaLatte, opts => opts.MapFrom(src => src.QuantitaLatte))
                ;
        }
    }

}
