namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCampoAbilitato : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AUTOCISTERNA", "ABILITATO", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AUTOCISTERNA", "ABILITATO");
        }
    }
}
