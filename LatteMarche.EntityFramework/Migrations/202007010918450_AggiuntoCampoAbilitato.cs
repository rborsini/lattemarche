namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCampoAbilitato : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Cessionario", newName: "ANAGRAFE_CESSIONARIO");
            AddColumn("dbo.ANAGRAFE_ACQUIRENTE", "ABILITATO", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.ANAGRAFE_ALLEVAMENTO", "ABILITATO", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.AUTOCISTERNA", "ABILITATO", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.ANAGRAFE_CESSIONARIO", "ABILITATO", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.ANAGRAFE_DESTINATARIO", "ABILITATO", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ANAGRAFE_DESTINATARIO", "ABILITATO");
            DropColumn("dbo.ANAGRAFE_CESSIONARIO", "ABILITATO");
            DropColumn("dbo.AUTOCISTERNA", "ABILITATO");
            DropColumn("dbo.ANAGRAFE_ALLEVAMENTO", "ABILITATO");
            DropColumn("dbo.ANAGRAFE_ACQUIRENTE", "ABILITATO");
            RenameTable(name: "dbo.ANAGRAFE_CESSIONARIO", newName: "Cessionario");
        }
    }
}
