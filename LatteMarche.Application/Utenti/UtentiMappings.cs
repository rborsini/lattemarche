using AutoMapper.Configuration;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Utenti
{
    public class UtentiMappings
    {
        internal static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
        {
           

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
                //.ForMember(dest => dest.CodiceAllevatore, opts => opts.MapFrom(src => src.CodiceAllevatore.Trim()))
                //.ForMember(dest => dest.QuantitaLatte, opts => opts.MapFrom(src => src.QuantitaLatte))
                .ForMember(dest => dest.Telefono, opts => opts.MapFrom(src => src.Telefono.Trim()))
                .ForMember(dest => dest.Cellulare, opts => opts.MapFrom(src => src.Cellulare.Trim()))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.Sesso, opts => opts.MapFrom(src => src.Sesso.Trim()))
                //.ForMember(dest => dest.IdTipoLatte, opts => opts.MapFrom(src => src.IdTipoLatte))
                //.ForMember(dest => dest.NumeroComunicazione, opts => opts.MapFrom(src => src.NumeroComunicazione.Trim()))
                .ForMember(dest => dest.Note, opts => opts.MapFrom(src => src.Note.Trim()))

                .ForMember(dest => dest.IdAcquirente, opts => opts.MapFrom(src => src.UtenteXAcquirente != null ? src.UtenteXAcquirente.IdAcquirente : (int?)null))
                .ForMember(dest => dest.IdDestinatario, opts => opts.MapFrom(src => src.UtenteXDestinatario != null ? src.UtenteXDestinatario.IdDestinatario : (int?)null))
                .ForMember(dest => dest.IdCessionario, opts => opts.MapFrom(src => src.UtenteXCessionario != null ? src.UtenteXCessionario.IdCessionario : (int?)null))
                .ForMember(dest => dest.IdAziendaTrasporti, opts => opts.MapFrom(src => src.TrasportatoreXAzienda != null ? src.TrasportatoreXAzienda.IdAzienda : (int?)null))
                .ForMember(dest => dest.Allevamenti, opts => opts.MapFrom(src => src.Allevamenti))
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
                //.ForMember(dest => dest.CodiceAllevatore, opts => opts.MapFrom(src => src.CodiceAllevatore.Trim()))
                //.ForMember(dest => dest.QuantitaLatte, opts => opts.MapFrom(src => src.QuantitaLatte))
                .ForMember(dest => dest.Telefono, opts => opts.MapFrom(src => src.Telefono.Trim()))
                .ForMember(dest => dest.Cellulare, opts => opts.MapFrom(src => src.Cellulare.Trim()))
                .ForMember(dest => dest.IdComune, opts => opts.MapFrom(src => src.IdComune))
                .ForMember(dest => dest.Sesso, opts => opts.MapFrom(src => src.Sesso.Trim()))
                //.ForMember(dest => dest.IdTipoLatte, opts => opts.MapFrom(src => src.IdTipoLatte))
                //.ForMember(dest => dest.NumeroComunicazione, opts => opts.MapFrom(src => src.NumeroComunicazione.Trim()))
                .ForMember(dest => dest.Note, opts => opts.MapFrom(src => src.Note.Trim()))

                .ForMember(dest => dest.UtenteXAcquirente, opts => opts.MapFrom(src => src.IdAcquirente.HasValue ? new UtenteXAcquirente() {  Id = src.Id, IdAcquirente = src.IdAcquirente.Value } : null))
                .ForMember(dest => dest.UtenteXCessionario, opts => opts.MapFrom(src => src.IdCessionario.HasValue ? new UtenteXCessionario() { Id = src.Id, IdCessionario = src.IdCessionario.Value } : null))
                .ForMember(dest => dest.UtenteXDestinatario, opts => opts.MapFrom(src => src.IdDestinatario.HasValue ? new UtenteXDestinatario() { Id = src.Id, IdDestinatario = src.IdDestinatario.Value } : null))
                .ForMember(dest => dest.TrasportatoreXAzienda, opts => opts.MapFrom(src => src.IdAziendaTrasporti.HasValue ? new TrasportatoreXAzienda() { Id = src.Id, IdAzienda = src.IdAziendaTrasporti.Value } : null))
                .ForMember(dest => dest.Allevamenti, opts => opts.MapFrom(src => src.Allevamenti))
                ;

            return mappings;
        }
    }
}
