using Autofac;
using System;
using LatteMarche.EntityFramework;
using LatteMarche.Core;
using LatteMarche.Application.Utenti.Services;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.Comuni.Services;
using LatteMarche.Application.Comuni.Interfaces;

namespace LatteMarche.Application
{

	/// <summary>
	/// Modulo AutoFac per la registrazione dei servizi
	/// </summary>
	public class ApplicationModule : Module
	{
		private bool isWeb;

		public ApplicationModule(bool isWeb = true)
		{
			this.isWeb = isWeb;
		}

		protected override void Load(ContainerBuilder builder)
		{
			AutomapperConfig.Configure();

			if (this.isWeb)
			{
				builder.RegisterModule(new DataModule());

				builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

				builder.RegisterType<UtentiService>().As<IUtentiService>().InstancePerRequest();
                builder.RegisterType<ComuniService>().As<IComuniService>().InstancePerRequest();


            }
			else
			{
				builder.RegisterModule(new DataModule(false));

				builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

				builder.RegisterType<UtentiService>().As<IUtentiService>();
                builder.RegisterType<ComuniService>().As<IComuniService>();
            }


			base.Load(builder);
		}
	}
}
