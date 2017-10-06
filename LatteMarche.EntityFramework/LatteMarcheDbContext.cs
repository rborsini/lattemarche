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
        public DbSet<Comune> Comuni { get; set; }
        public DbSet<TipoLatte> TipiLatte { get; set; }
        public DbSet<TipoProfilo> TipiProfilo { get; set; }
        public DbSet<V_Allevatore> Allevatore { get; set; }
        public DbSet<Allevamento> Allevamenti { get; set; }
        public DbSet<V_Trasportatore> Trasportatori { get; set; }
        public DbSet<Giro> Giri { get; set; }
        public DbSet<AllevamentoXGiro> AllevamentiXGiro { get; set; }
        public DbSet<LaboratorioAnalisi> LaboratoriAnalisi { get; set; }


        public LatteMarcheDbContext()
			: base("name=LatteMarcheDbContext")
		{
			Database.SetInitializer<LatteMarcheDbContext>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

    }
}
