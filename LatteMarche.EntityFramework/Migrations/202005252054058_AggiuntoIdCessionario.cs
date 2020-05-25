namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoIdCessionario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PRELIEVO_LATTE", "ID_CESSIONARIO", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PRELIEVO_LATTE", "ID_CESSIONARIO");
        }
    }
}
