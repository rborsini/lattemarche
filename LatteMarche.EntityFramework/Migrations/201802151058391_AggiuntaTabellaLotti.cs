namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaTabellaLotti : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LOTTI",
                c => new
                    {
                        ID_LOTTO = c.Long(nullable: false, identity: true),
                        CODICE = c.String(),
                        CODICE_SITRA = c.String(),
                        TS = c.DateTime(nullable: false),
                        INVIATO = c.Boolean(nullable: false),
                        ERRORE = c.Boolean(nullable: false),
                        MESSAGGIO = c.String(),
                    })
                .PrimaryKey(t => t.ID_LOTTO);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LOTTI");
        }
    }
}
