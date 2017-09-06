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
        public string Username { get; set; }
        public string Password { get; set; }
        public int IdProfilo { get; set; }
        public bool? Abilitato { get; set; }
        public bool? Visibile { get; set; }
        public string RagioneSociale { get; set; }
        public string CodiceAllevatore { get; set; }
        public int QuantitaLatte { get; set; }
        public string Telefono { get; set; }
        public string Cellulare { get; set; }
        public int IdComune { get; set; }
        public string Sesso { get; set; }
        public int IdTipoLatte { get; set; }
        public string NumeroComunicazione { get; set; }
        public string Note { get; set; }
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
                .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.Username.Trim()))
                .ForMember(dest => dest.Password, opts => opts.MapFrom(src => src.Password.Trim()))
                .ForMember(dest => dest.IdProfilo, opts => opts.MapFrom(src => src.IdProfilo))
                .ForMember(dest => dest.Abilitato, opts => opts.MapFrom(src => src.Abilitato))
                .ForMember(dest => dest.Visibile, opts => opts.MapFrom(src => src.Visibile))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.CodiceAllevatore, opts => opts.MapFrom(src => src.CodiceAllevatore.Trim()))
                .ForMember(dest => dest.QuantitaLatte, opts => opts.MapFrom(src => src.QuantitaLatte))
                .ForMember(dest => dest.Telefono, opts => opts.MapFrom(src => src.Telefono.Trim()))
                .ForMember(dest => dest.Cellulare, opts => opts.MapFrom(src => src.Cellulare.Trim()))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.Sesso, opts => opts.MapFrom(src => src.Sesso.Trim()))
                .ForMember(dest => dest.IdTipoLatte, opts => opts.MapFrom(src => src.IdTipoLatte))
                .ForMember(dest => dest.NumeroComunicazione, opts => opts.MapFrom(src => src.NumeroComunicazione.Trim()))
                .ForMember(dest => dest.Note, opts => opts.MapFrom(src => src.Note.Trim()))
                ;
            Mapper.CreateMap<UtenteDto, Utente>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                .ForMember(dest => dest.Cognome, opts => opts.MapFrom(src => src.Cognome.Trim()))
                .ForMember(dest => dest.PivaCF, opts => opts.MapFrom(src => src.PivaCF.Trim()))
                .ForMember(dest => dest.Indirizzo, opts => opts.MapFrom(src => src.Indirizzo.Trim()))
                .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.Username.Trim()))
                .ForMember(dest => dest.Password, opts => opts.MapFrom(src => src.Password.Trim()))
                .ForMember(dest => dest.IdProfilo, opts => opts.MapFrom(src => src.IdProfilo))
                .ForMember(dest => dest.Abilitato, opts => opts.MapFrom(src => src.Abilitato))
                .ForMember(dest => dest.Visibile, opts => opts.MapFrom(src => src.Visibile))
                .ForMember(dest => dest.RagioneSociale, opts => opts.MapFrom(src => src.RagioneSociale.Trim()))
                .ForMember(dest => dest.CodiceAllevatore, opts => opts.MapFrom(src => src.CodiceAllevatore.Trim()))
                .ForMember(dest => dest.QuantitaLatte, opts => opts.MapFrom(src => src.QuantitaLatte))
                .ForMember(dest => dest.Telefono, opts => opts.MapFrom(src => src.Telefono.Trim()))
                .ForMember(dest => dest.Cellulare, opts => opts.MapFrom(src => src.Cellulare.Trim()))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.Sesso, opts => opts.MapFrom(src => src.Sesso.Trim()))
                .ForMember(dest => dest.IdTipoLatte, opts => opts.MapFrom(src => src.IdTipoLatte))
                .ForMember(dest => dest.NumeroComunicazione, opts => opts.MapFrom(src => src.NumeroComunicazione.Trim()))
                .ForMember(dest => dest.Note, opts => opts.MapFrom(src => src.Note.Trim()))
                ;
        }
    }

}
