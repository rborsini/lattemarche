namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntiCampiTabellaPrelievi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PRELIEVO_LATTE", "LATITUDINE", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PRELIEVO_LATTE", "LONGITUDINE", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PRELIEVO_LATTE", "ID_AUTOCISTERNA", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PRELIEVO_LATTE", "ID_AUTOCISTERNA");
            DropColumn("dbo.PRELIEVO_LATTE", "LONGITUDINE");
            DropColumn("dbo.PRELIEVO_LATTE", "LATITUDINE");
        }
    }
}
