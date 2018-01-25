namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntiCampiSynchPrelievo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PRELIEVO_LATTE", "LastOperation", c => c.Int(nullable: false));
            AddColumn("dbo.PRELIEVO_LATTE", "LastChange", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ANAGRAFE_DESTINATARIO", "ID_COMUNE", c => c.Int());
            AlterColumn("dbo.ANAGRAFE_DESTINATARIO", "IDSITRA_STABILIMENTO_CASEIFICIO", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ANAGRAFE_DESTINATARIO", "IDSITRA_STABILIMENTO_CASEIFICIO", c => c.Int(nullable: false));
            AlterColumn("dbo.ANAGRAFE_DESTINATARIO", "ID_COMUNE", c => c.Int(nullable: false));
            DropColumn("dbo.PRELIEVO_LATTE", "LastChange");
            DropColumn("dbo.PRELIEVO_LATTE", "LastOperation");
        }
    }
}
