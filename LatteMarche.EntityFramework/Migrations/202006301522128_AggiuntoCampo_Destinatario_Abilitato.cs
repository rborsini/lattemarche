namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCampo_Destinatario_Abilitato : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ANAGRAFE_DESTINATARIO", "ABILITATO", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ANAGRAFE_DESTINATARIO", "ABILITATO");
        }
    }
}
