namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiunteRelazioniAcquirenti : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.UTENTE_X_ACQUIRENTE", "ID_ACQUIRENTE");
            AddForeignKey("dbo.UTENTE_X_ACQUIRENTE", "ID_ACQUIRENTE", "dbo.ANAGRAFE_ACQUIRENTE", "ID_ACQUIRENTE", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UTENTE_X_ACQUIRENTE", "ID_ACQUIRENTE", "dbo.ANAGRAFE_ACQUIRENTE");
            DropIndex("dbo.UTENTE_X_ACQUIRENTE", new[] { "ID_ACQUIRENTE" });
        }
    }
}
