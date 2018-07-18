namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCodiceSitraPrelievo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PRELIEVO_LATTE", "CODICE_SITRA", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PRELIEVO_LATTE", "CODICE_SITRA");
        }
    }
}
