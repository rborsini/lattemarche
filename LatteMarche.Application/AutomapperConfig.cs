using System;
using AutoMapper;
using System.Linq.Expressions;
using AutoMapper.Configuration;
using LatteMarche.Application.Acquirenti;
using LatteMarche.Application.AnalisiLatte;
using LatteMarche.Application.Trasportatori;
using LatteMarche.Application.Allevamenti;
using LatteMarche.Application.Auth;
using LatteMarche.Application.Comuni;
using LatteMarche.Application.Destinatari;
using LatteMarche.Application.Documenti;
using LatteMarche.Application.Logs;
using LatteMarche.Application.PrelieviLatte;

namespace LatteMarche.Application
{
    /// <summary>
    /// Configurazione dei mapping da Entity e Dto
    /// </summary>
    public static class AutomapperConfig
	{
		public static MapperConfigurationExpression Configure(MapperConfigurationExpression mappings)
		{

            mappings = AcquirentiMappings.Configure(mappings);
            mappings = AllevamentiMappings.Configure(mappings);
            mappings = AnalisiMappings.Configure(mappings);
            mappings = AuthMappings.Configure(mappings);
            mappings = ComuniMappings.Configure(mappings);
            mappings = DestinatarioMappings.Configure(mappings);
            mappings = DocumentiMappings.Configure(mappings);
            mappings = LogsMappings.Configure(mappings);
            mappings = PrelieviLatteMappings.Configure(mappings);
            mappings = TrasportatoriMappings.Configure(mappings);

            return mappings;
            
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
