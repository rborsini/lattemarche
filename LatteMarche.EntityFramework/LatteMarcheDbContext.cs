using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatteMarche.Core;
using LatteMarche.Core.Models;

namespace LatteMarche.EntityFramework
{
	public class LatteMarcheDbContext : DbContext, IContext
	{
		public DbSet<Utente> Utenti { get; set; }

		public LatteMarcheDbContext()
			: base("name=LatteMarcheDbContext")
		{
			Database.SetInitializer<LatteMarcheDbContext>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}

	}
}
