using AutoMapper;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Ruoli.Dtos
{
    public class RuoloDto : EntityDto
    {
        public long Id { get; set; }
        public string Codice { get; set; }
        public string Descrizione { get; set; }

        public List<Pagina> Pagine_MVC { get; set; }
        public List<Pagina> Pagine_API { get; set; }
        public List<UtenteDto> Utenti { get; set; }

        public RuoloDto()
        {
            this.Pagine_MVC = new List<Pagina>();
            this.Pagine_API = new List<Pagina>();
        }

        public class Pagina
        {
            public bool Enabled { get; set; }
            public string Title { get; set; }

            public List<ViewItemDto> Items { get; set; }

            public Pagina()
            {
                this.Items = new List<ViewItemDto>();
            }

            public Pagina(bool enabled, string title)
                : this()
            {
                this.Enabled = enabled;
                this.Title = title;
            }

        }

        public class ViewItemDto
        {
            public bool Enabled { get; set; }
            public string Title { get; set; }
            public string DisplayName { get; set; }

            public ViewItemDto() { }

            public ViewItemDto(bool enabled, string title, string displayName)
            {
                this.Enabled = enabled;
                this.Title = title;
                this.DisplayName = displayName;
            }

        }

    }




    public class RuoloMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<Ruolo, RuoloDto>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Codice, opts => opts.MapFrom(src => src.Codice.Trim()))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                ;

            Mapper.CreateMap<RuoloDto, Ruolo>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Codice, opts => opts.MapFrom(src => src.Codice.Trim()))
                .ForMember(dest => dest.Descrizione, opts => opts.MapFrom(src => src.Descrizione.Trim()))
                ;
        }
    }
}
