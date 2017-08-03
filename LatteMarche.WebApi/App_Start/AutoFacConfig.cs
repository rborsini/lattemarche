using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using LatteMarche.Application;
using System.Web.Mvc;

namespace LatteMarche.WebApi.App_Start
{
	public class AutoFacConfig
	{
		internal static IContainer Container;

		internal static void Configure()
		{
			ContainerBuilder builder = new ContainerBuilder();

			HttpConfiguration config = GlobalConfiguration.Configuration;

			// This line tells autofac, when a controller is initialized, pass into its constructor, the implementations of the required interfaces
			builder.RegisterControllers(Assembly.GetExecutingAssembly());
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

			// Registro i db context e unit of work
			builder.RegisterModule(new ApplicationModule());

			builder.RegisterFilterProvider();
			builder.RegisterWebApiFilterProvider(config);

			Container = builder.Build();

			config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
			DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));

		}
	}
}
