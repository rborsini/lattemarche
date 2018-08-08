namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCampoDuration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Duration", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "Duration");
        }
    }
}
