using AutoMapper;
using AutoMapper.Configuration;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Rest.Dtos;
using LatteMarche.Xamarin.ViewModels.Prelievi;
using System;
using System.Collections.Generic;
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
				.ForMember(dest => dest.DataConsegna, opt => opt.MapFrom(src => src.Giro.DataConsegna))
				.ForMember(dest => dest.Quantita, opt => opt.MapFrom(src => src.Quantita_kg))
				.ForMember(dest => dest.IdLabAnalisi, opt => opt.MapFrom(src => 2))		// LAB ANALISI ASSAM
				;
			#endregion

			Mapper.Reset();
			Mapper.Initialize(mappings);
		}
	}
}
