namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntiCampiDispositivoMobile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DISPOSITIVI_MOBILI", "MODELLO", c => c.String());
            AddColumn("dbo.DISPOSITIVI_MOBILI", "MARCA", c => c.String());
            AddColumn("dbo.DISPOSITIVI_MOBILI", "VERSIONE_OS", c => c.String());
            AddColumn("dbo.DISPOSITIVI_MOBILI", "NOME", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DISPOSITIVI_MOBILI", "NOME");
            DropColumn("dbo.DISPOSITIVI_MOBILI", "VERSIONE_OS");
            DropColumn("dbo.DISPOSITIVI_MOBILI", "MARCA");
            DropColumn("dbo.DISPOSITIVI_MOBILI", "MODELLO");
        }
    }
}
