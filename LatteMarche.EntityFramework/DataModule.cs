using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LatteMarche.Core;

namespace LatteMarche.EntityFramework
{
	public class DataModule : Module
	{
		private bool isWeb;

		public DataModule(bool isWeb = true)
		{
			this.isWeb = isWeb;
		}

		protected override void Load(ContainerBuilder builder)
		{

			if (this.isWeb)
			{
				builder.RegisterType<LatteMarcheDbContext>().As<IContext>().InstancePerRequest();
				builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
			}
			else
			{
				builder.RegisterType<LatteMarcheDbContext>().As<IContext>();
				builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
			}

			base.Load(builder);
		}
	}
}
