namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Configuration;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCessionario : DbMigration
    {

        private bool IsTestEnv => ConfigurationManager.ConnectionStrings["LatteMarcheDbContext"].ConnectionString.Contains("TEST");

        public override void Up()
        {
            CreateTable(
                "dbo.UTENTE_X_CESSIONARIO",
                c => new
                    {
                        ID_UTENTE = c.Int(nullable: false),
                        ID_CESSIONARIO = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID_UTENTE)
                .ForeignKey("dbo.ANAGRAFE_CESSIONARIO", t => t.ID_CESSIONARIO, cascadeDelete: true)
                .ForeignKey("dbo.UTENTI", t => t.ID_UTENTE)
                .Index(t => t.ID_UTENTE)
                .Index(t => t.ID_CESSIONARIO);
            
            CreateTable(
                "dbo.ANAGRAFE_CESSIONARIO",
                c => new
                    {
                        ID_CESSIONARIO = c.Int(nullable: false, identity: true),
                        RAG_SOC_CESSIONARIO = c.String(),
                        PIVA_CESSIONARIO = c.String(),
                        INDIRIZZO_CESSIONARIO = c.String(),
                        ID_COMUNE = c.Int(),
                    })
                .PrimaryKey(t => t.ID_CESSIONARIO)
                .ForeignKey("dbo.COMUNI", t => t.ID_COMUNE)
                .Index(t => t.ID_COMUNE);

            if (this.IsTestEnv)
            {

                CreateTable(
                    "dbo.AZIENDA_TRASPORTATORI",
                    c => new
                    {
                        ID_AZIENDA_TRASPORTATORI = c.Int(nullable: false, identity: true),
                        P_IVA = c.String(),
                        NOME_TITOLARE = c.String(),
                        COGNOME_TITOLARE = c.String(),
                        RAGIONE_SOCIALE = c.String(),
                    })
                    .PrimaryKey(t => t.ID_AZIENDA_TRASPORTATORI);

                CreateTable(
                    "dbo.TRASPORTATORI_X_AZIENDA",
                    c => new
                    {
                        ID_UTENTE = c.Int(nullable: false),
                        ID_AZIENDA_TRASPORTATORI = c.Int(nullable: false),
                    })
                    .PrimaryKey(t => t.ID_UTENTE)
                    .ForeignKey("dbo.UTENTI", t => t.ID_UTENTE)
                    .Index(t => t.ID_UTENTE);

            }
            
            CreateIndex("dbo.UTENTE_X_DESTINATARIO", "ID_DESTINATARIO");
            AddForeignKey("dbo.UTENTE_X_DESTINATARIO", "ID_DESTINATARIO", "dbo.ANAGRAFE_DESTINATARIO", "ID_DESTINATARIO", cascadeDelete: true);
        }
        
        public override void Down()
        {
            if(this.IsTestEnv)
                DropForeignKey("dbo.TRASPORTATORI_X_AZIENDA", "ID_UTENTE", "dbo.UTENTI");

            DropForeignKey("dbo.UTENTE_X_DESTINATARIO", "ID_DESTINATARIO", "dbo.ANAGRAFE_DESTINATARIO");
            DropForeignKey("dbo.UTENTE_X_CESSIONARIO", "ID_UTENTE", "dbo.UTENTI");
            DropForeignKey("dbo.UTENTE_X_CESSIONARIO", "ID_CESSIONARIO", "dbo.Cessionario");
            DropForeignKey("dbo.Cessionario", "ID_COMUNE", "dbo.COMUNI");

            if (this.IsTestEnv)
                DropIndex("dbo.TRASPORTATORI_X_AZIENDA", new[] { "ID_UTENTE" });

            DropIndex("dbo.UTENTE_X_DESTINATARIO", new[] { "ID_DESTINATARIO" });
            DropIndex("dbo.Cessionario", new[] { "ID_COMUNE" });
            DropIndex("dbo.UTENTE_X_CESSIONARIO", new[] { "ID_CESSIONARIO" });
            DropIndex("dbo.UTENTE_X_CESSIONARIO", new[] { "ID_UTENTE" });

            if (this.IsTestEnv)
            {
                DropTable("dbo.TRASPORTATORI_X_AZIENDA");
                DropTable("dbo.AZIENDA_TRASPORTATORI");
            }

            DropTable("dbo.Cessionario");
            DropTable("dbo.UTENTE_X_CESSIONARIO");
        }
    }
}
