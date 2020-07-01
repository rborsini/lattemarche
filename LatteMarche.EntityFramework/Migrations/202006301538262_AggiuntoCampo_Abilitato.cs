namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCampo_Abilitato : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ANAGRAFE_ACQUIRENTE", "ABILITATO", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.ANAGRAFE_ALLEVAMENTO", "ABILITATO", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.Cessionario", "ABILITATO", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cessionario", "ABILITATO");
            DropColumn("dbo.ANAGRAFE_ALLEVAMENTO", "ABILITATO");
            DropColumn("dbo.ANAGRAFE_ACQUIRENTE", "ABILITATO");
        }
    }
}
