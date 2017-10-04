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
using LatteMarche.Application.AllevamentiXGiro.Dtos;




namespace LatteMarche.Application
{
	/// <summary>
	/// Configurazione dei mapping da Entity e Dto
	/// </summary>
	public static class AutomapperConfig
	{
		public static void Configure()
		{
			UtentiMappings.Configure();
            ComuniMappings.Configure();
            TipiLatteMappings.Configure();
            TipiProfiloMappings.Configure();
            AllevatoriMappings.Configure();
            AllevamentiMappings.Configure();
            TrasportatoriMappings.Configure();
            GiriMappings.Configure();
            AllevamentiXGiroMappings.Configure();
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
