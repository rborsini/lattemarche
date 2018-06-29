using System;
using AutoMapper;
using System.Linq.Expressions;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.Application.TipiLatte.Dtos;
using LatteMarche.Application.TipiProfilo.Dtos;
using LatteMarche.Application.Allevatori.Dtos;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Trasportatori.Dtos;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.LaboratoriAnalisi.Dtos;
using LatteMarche.Application.Acquirenti.Dtos;
using LatteMarche.Application.Destinatari.Dtos;
using LatteMarche.Application.Lotti.Dtos;
using LatteMarche.Application.Ruoli.Dtos;

namespace LatteMarche.Application
{
    /// <summary>
    /// Configurazione dei mapping da Entity e Dto
    /// </summary>
    public static class AutomapperConfig
	{
		public static void Configure()
		{

            AcquirentiMappings.Configure();
            AllevamentiMappings.Configure();
            AllevatoriMappings.Configure();
            AzioneMappings.Configure();
            ComuniMappings.Configure();
            DestinatarioMappings.Configure();
            GiriMappings.Configure();
            LaboratoriAnalisiMappings.Configure();
            LottiMappings.Configure();
            PrelieviLatteMappings.Configure();
            RuoloMappings.Configure();
            TipiLatteMappings.Configure();
            TipiProfiloMappings.Configure();
            TrasportatoriMappings.Configure();
            UtentiMappings.Configure();
            
        }
	}

	public static class MappingExpressionExtensions
	{
		public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
		{
			expression.ForAllMembers(opt => opt.Ignore());
			return expression;
		}

		public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, Expression<Func<TDestination, object>> selector)
		{
			map.ForMember(selector, config => config.Ignore());
			return map;
		}
	}



}
