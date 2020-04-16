namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaTabellaDispositiviMobili : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DISPOSITIVI_MOBILI",
                c => new
                    {
                        IMEI = c.String(nullable: false, maxLength: 128),
                        USERNAME = c.String(),
                        ATTIVO = c.Boolean(nullable: false),
                        DATA_REGISTRAZIONE = c.DateTime(nullable: false),
                        DATA_ULTIMO_DOWNLOAD = c.DateTime(),
                        DATA_ULTIMO_UPLOAD = c.DateTime(),
                        VERSIONE_APP = c.String(),
                        LATITUDINE = c.Decimal(precision: 18, scale: 2),
                        LONGITUDINE = c.Decimal(precision: 18, scale: 2),
                        ID_GIRO = c.Int(),
                    })
                .PrimaryKey(t => t.IMEI);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DISPOSITIVI_MOBILI");
        }
    }
}
