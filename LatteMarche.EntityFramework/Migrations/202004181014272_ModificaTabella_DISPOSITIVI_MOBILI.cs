namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificaTabella_DISPOSITIVI_MOBILI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DISPOSITIVI_MOBILI", "ID_TRASPORTATORE", c => c.Int());
            DropColumn("dbo.DISPOSITIVI_MOBILI", "USERNAME");
            DropColumn("dbo.DISPOSITIVI_MOBILI", "ID_GIRO");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DISPOSITIVI_MOBILI", "ID_GIRO", c => c.Int());
            AddColumn("dbo.DISPOSITIVI_MOBILI", "USERNAME", c => c.String());
            DropColumn("dbo.DISPOSITIVI_MOBILI", "ID_TRASPORTATORE");
        }
    }
}
