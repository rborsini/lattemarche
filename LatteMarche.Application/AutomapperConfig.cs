using AutoMapper.Configuration;
using LatteMarche.Application.AnalisiLatte;
using LatteMarche.Application.Auth;
using LatteMarche.Application.Comuni;
using LatteMarche.Application.Logs;
using LatteMarche.Application.PrelieviLatte;
using LatteMarche.Application.Dispositivi;
using LatteMarche.Application.Utenti;
using LatteMarche.Application.Acquirenti;
using LatteMarche.Application.Destinatari;
using LatteMarche.Application.Cessionari;
using LatteMarche.Application.Trasportatori;
using LatteMarche.Application.Allevamenti;
using LatteMarche.Application.Giri;
using LatteMarche.Application.Dashboard;

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
            mappings = AutocisterneMappings.Configure(mappings);
            mappings = CessionariMappings.Configure(mappings);
            mappings = ComuniMappings.Configure(mappings);
            mappings = DestinatarioMappings.Configure(mappings);
            mappings = DispositiviMappings.Configure(mappings);
            mappings = GiriMappings.Configure(mappings);
            mappings = LogsMappings.Configure(mappings);
            mappings = PrelieviLatteMappings.Configure(mappings);
            mappings = UtentiMappings.Configure(mappings);
            mappings = WidgetsMappings.Configure(mappings);

            return mappings;
            
        }
	}




}
