namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaRelazione_Dispositivi_Utenti : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.DISPOSITIVI_MOBILI", "ID_TRASPORTATORE");
            AddForeignKey("dbo.DISPOSITIVI_MOBILI", "ID_TRASPORTATORE", "dbo.UTENTI", "ID_UTENTE");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DISPOSITIVI_MOBILI", "ID_TRASPORTATORE", "dbo.UTENTI");
            DropIndex("dbo.DISPOSITIVI_MOBILI", new[] { "ID_TRASPORTATORE" });
        }
    }
}
