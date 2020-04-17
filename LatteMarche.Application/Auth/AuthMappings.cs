using AutoMapper.Configuration;
using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Auth
{
    public class AuthMappings
    {
        internal static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
            mappings.CreateMap<Azione, AzioneDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.Type.Trim()))
                .ForMember(dest => dest.Controller, opts => opts.MapFrom(src => src.Controller.Trim()))
                .ForMember(dest => dest.Action, opts => opts.MapFrom(src => src.Action.Trim()))
                .ForMember(dest => dest.ViewItem, opts => opts.MapFrom(src => src.ViewItem.Trim()))
                .ForMember(dest => dest.Pagina, opts => opts.MapFrom(src => src.Pagina.Trim()))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                ;

            mappings.CreateMap<AzioneDto, Azione>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.Type.Trim()))
                .ForMember(dest => dest.Controller, opts => opts.MapFrom(src => src.Controller.Trim()))
                .ForMember(dest => dest.Action, opts => opts.MapFrom(src => src.Action.Trim()))
                .ForMember(dest => dest.ViewItem, opts => opts.MapFrom(src => src.ViewItem.Trim()))
                .ForMember(dest => dest.Pagina, opts => opts.MapFrom(src => src.Pagina.Trim()))
                .ForMember(dest => dest.Nome, opts => opts.MapFrom(src => src.Nome.Trim()))
                ;

            mappings.CreateMap<Ruolo, RuoloDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Codice, opts => opts.MapFrom(src => src.Codice.Trim()))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                ;

            mappings.CreateMap<RuoloDto, Ruolo>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Codice, opts => opts.MapFrom(src => src.Codice.Trim()))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                ;

            mappings.CreateMap<TipoProfilo, TipoProfiloDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                ;

            mappings.CreateMap<Utente, UtenteDto>()
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
            mappings.CreateMap<UtenteDto, Utente>()
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

            return mappings;
        }
    }
}
