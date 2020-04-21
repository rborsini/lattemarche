using AutoMapper;
using AutoMapper.Configuration;

namespace LatteMarche.WebApi.App_Start
{
	public class AutoMapperConfig
	{
		public static void Configure()
		{
			var mappings = new MapperConfigurationExpression();

			mappings = LatteMarche.Application.AutomapperConfig.Configure(mappings);
			mappings = LatteMarche.Application.Mobile.AutomapperConfig.Configure(mappings);

			Mapper.Reset();
			Mapper.Initialize(mappings);
		}

	}
}
