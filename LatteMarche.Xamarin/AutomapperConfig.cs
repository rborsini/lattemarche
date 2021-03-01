using AutoMapper;
using AutoMapper.Configuration;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Rest.Dtos;
using LatteMarche.Xamarin.ViewModels.Giri;
using LatteMarche.Xamarin.ViewModels.Prelievi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LatteMarche.Xamarin
{
    public class AutomapperConfig
    {
		public static void Configure()
		{
			var mappings = new MapperConfigurationExpression();

            #region Dtos

            mappings.CreateMap<TrasportatoreDto, Trasportatore>();
			mappings.CreateMap<AutocisternaDto, AutoCisterna>();
			
			mappings.CreateMap<TipoLatteDto, TipoLatte>()
				.ForMember(dest => dest.Codice, opt => opt.MapFrom(src => src.DescrizioneBreve));

			mappings.CreateMap<AcquirenteDto, Acquirente>();
			mappings.CreateMap<DestinatarioDto, Destinatario>();

			mappings.CreateMap<AllevamentoDto, Allevamento>();
			
			mappings.CreateMap<GiroDto, TemplateGiro>();

			mappings.CreateMap<Prelievo, PrelievoLatteDto>()
				.ForMember(dest => dest.LottoConsegna, opt => opt.MapFrom(src => src.Giro.CodiceLotto))
				.ForMember(dest => dest.Quantita, opt => opt.MapFrom(src => src.Quantita_kg))
				.ForMember(dest => dest.IdLabAnalisi, opt => opt.MapFrom(src => 2))     // LAB ANALISI ASSAM
				.ForMember(dest => dest.IdGiro, opt => opt.MapFrom(src => src.Giro.IdTemplateGiro))
				;

			mappings.CreateMap<PrelievoLatteDto, Prelievo>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
				.ForMember(dest => dest.Quantita_kg, opt => opt.MapFrom(src => src.Quantita))
				;

			#endregion

			#region ViewModels

			mappings.CreateMap<Giro, ViewModels.Giri.ItemViewModel>()
				.ForMember(dest => dest.SubTotaleStr, opt => opt.MapFrom(src => $"{src.Prelievi.Sum(p => p.Quantita_kg)} kg - {src.Prelievi.Sum(p => p.Quantita_lt)} lt"));

			mappings.CreateMap<Prelievo, ViewModels.Prelievi.ItemViewModel>();

			mappings.CreateMap<TrasbordoDto, ViewModels.Trasbordi.ItemViewModel>()
				.ForMember(dest => dest.TargaOrigine, opt => opt.MapFrom(src => src.Targa_Origine))
				.ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
				.ForMember(dest => dest.NumeroPrelievi, opt => opt.MapFrom(src => src.Prelievi.Count))
				.ForMember(dest => dest.Dto, opt => opt.MapFrom(src => src))
				;

			#endregion

			Mapper.Reset();
			Mapper.Initialize(mappings);
		}
	}
}
