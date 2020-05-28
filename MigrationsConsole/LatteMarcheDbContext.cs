using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.IO;
using LatteMarche.Xamarin.Db.Models;

namespace LatteMarche.Xamarin.Db
{
    public class LatteMarcheDbContext : DbContext
    {
		public DbSet<Acquirente> Acquirenti { get; set; }
		public DbSet<Allevamento> Allevamenti { get; set; }
		public DbSet<AutoCisterna> AutoCisterne { get; set; }
		public DbSet<Cessionario> Cessionari { get; set; }
		public DbSet<Destinatario> Destinatari { get; set; }
		public DbSet<Giro> Giri { get; set; }
		public DbSet<Prelievo> Prelievi { get; set; }
		public DbSet<Sincronizzazione> Sincronizzazioni { get; set; }
		public DbSet<Stampante> Stampanti { get; set; }
		public DbSet<TemplateGiro> TemplateGiri { get; set; }
		public DbSet<TipoLatte> TipiLatte { get; set; }
		public DbSet<Trasportatore> Trasportatori { get; set; }

		private const string databaseName = "database.db";
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{		
			//String databasePath = "";
			//switch (Device.RuntimePlatform)
			//{
			//	case Device.iOS:
			//		SQLitePCL.Batteries_V2.Init();
			//		databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName); ;
			//		break;
			//	case Device.Android:
			//		databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);
			//		break;
			//	default:
			//		throw new NotImplementedException("Platform not supported");
			//}
			// Specify that we will use sqlite and the path of the database here
			optionsBuilder.UseSqlite("Data Source=blah.db");
		}
	}
}
