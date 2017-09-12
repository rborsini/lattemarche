using System;
using AutoMapper;
using System.Linq.Expressions;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Comuni.Dtos;

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
