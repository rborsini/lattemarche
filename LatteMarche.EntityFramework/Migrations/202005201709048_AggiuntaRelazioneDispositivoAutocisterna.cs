namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaRelazioneDispositivoAutocisterna : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DISPOSITIVI_MOBILI", "ID_AUTOCISTERNA", c => c.Int());
            CreateIndex("dbo.DISPOSITIVI_MOBILI", "ID_AUTOCISTERNA");
            AddForeignKey("dbo.DISPOSITIVI_MOBILI", "ID_AUTOCISTERNA", "dbo.AUTOCISTERNA", "ID_VEICOLO");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DISPOSITIVI_MOBILI", "ID_AUTOCISTERNA", "dbo.AUTOCISTERNA");
            DropIndex("dbo.DISPOSITIVI_MOBILI", new[] { "ID_AUTOCISTERNA" });
            DropColumn("dbo.DISPOSITIVI_MOBILI", "ID_AUTOCISTERNA");
        }
    }
}
