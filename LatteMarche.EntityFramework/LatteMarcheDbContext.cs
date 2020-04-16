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
        public DbSet<Autocisterna> Autocisterne { get; set; }
        public DbSet<Autorizzazione> Autorizzazioni { get; set; }
        public DbSet<Azione> Azioni { get; set; }
        public DbSet<Analisi> Analisi { get; set; }
        public DbSet<ValoreAnalisi> ValoriAnalisi { get; set; }
        public DbSet<Utente> Utenti { get; set; }
        public DbSet<Comune> Comuni { get; set; }
        public DbSet<Documento> Documenti { get; set; }
        public DbSet<TipoLatte> TipiLatte { get; set; }
        public DbSet<TipoProfilo> TipiProfilo { get; set; }
        public DbSet<V_Allevatore> Allevatore { get; set; }
        public DbSet<Allevamento> Allevamenti { get; set; }
        public DbSet<V_Allevamento> V_Allevamenti { get; set; }
        public DbSet<V_Trasportatore> Trasportatori { get; set; }
        public DbSet<Giro> Giri { get; set; }
        public DbSet<AllevamentoXGiro> AllevamentiXGiro { get; set; }
        public DbSet<LaboratorioAnalisi> LaboratoriAnalisi { get; set; }
        public DbSet<PrelievoLatte> PrelieviLatte { get; set; }
        public DbSet<V_PrelievoLatte> V_PrelieviLatte { get; set; }
        public DbSet<Acquirente> Acquirenti { get; set; }
        public DbSet<Destinatario> Destinatari { get; set; }
        public DbSet<Lotto> Lotti { get; set; }
        public DbSet<Ruolo> Ruoli { get; set; }
        public DbSet<RuoloUtente> RuoliUtente { get; set; }
        public DbSet<LogRecord> Logs { get; set; }
        public DbSet<UtenteXAcquirente> UtentiXAcquirente { get; set; }
        public DbSet<UtenteXDestinatario> UtentiXDestinatario { get; set; }

        public DbSet<DispositivoMobile> DispositiviMobile { get; set; }

        public LatteMarcheDbContext()
			: base("name=LatteMarcheDbContext")
		{
			Database.SetInitializer<LatteMarcheDbContext>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<TipoLatte>().Property(x => x.FattoreConversione).HasPrecision(18, 3);
		}

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

    }
}
