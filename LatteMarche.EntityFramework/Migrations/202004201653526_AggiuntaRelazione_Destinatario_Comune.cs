namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaRelazione_Destinatario_Comune : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ANAGRAFE_DESTINATARIO", "ID_COMUNE");
            AddForeignKey("dbo.ANAGRAFE_DESTINATARIO", "ID_COMUNE", "dbo.COMUNI", "ID_COMUNE");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ANAGRAFE_DESTINATARIO", "ID_COMUNE", "dbo.COMUNI");
            DropIndex("dbo.ANAGRAFE_DESTINATARIO", new[] { "ID_COMUNE" });
        }
    }
}
