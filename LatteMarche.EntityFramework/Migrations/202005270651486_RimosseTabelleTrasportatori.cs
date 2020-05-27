namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RimosseTabelleTrasportatori : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.TRASPORTATORI_X_AZIENDA", "ID_UTENTE", "dbo.UTENTI");
            //DropIndex("dbo.TRASPORTATORI_X_AZIENDA", new[] { "ID_UTENTE" });
            //DropTable("dbo.TRASPORTATORI_X_AZIENDA");
            //DropTable("dbo.AZIENDA_TRASPORTATORI");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.AZIENDA_TRASPORTATORI",
            //    c => new
            //        {
            //            ID_AZIENDA_TRASPORTATORI = c.Int(nullable: false, identity: true),
            //            P_IVA = c.String(),
            //            NOME_TITOLARE = c.String(),
            //            COGNOME_TITOLARE = c.String(),
            //            RAGIONE_SOCIALE = c.String(),
            //        })
            //    .PrimaryKey(t => t.ID_AZIENDA_TRASPORTATORI);
            
            //CreateTable(
            //    "dbo.TRASPORTATORI_X_AZIENDA",
            //    c => new
            //        {
            //            ID_UTENTE = c.Int(nullable: false),
            //            ID_AZIENDA_TRASPORTATORI = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ID_UTENTE);
            
            //CreateIndex("dbo.TRASPORTATORI_X_AZIENDA", "ID_UTENTE");
            //AddForeignKey("dbo.TRASPORTATORI_X_AZIENDA", "ID_UTENTE", "dbo.UTENTI", "ID_UTENTE");
        }
    }
}
