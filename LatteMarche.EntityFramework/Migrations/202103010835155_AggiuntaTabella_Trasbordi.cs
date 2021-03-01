namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaTabella_Trasbordi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TRASBORDI",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IMEI_Origine = c.String(),
                        Targa_Origine = c.String(),
                        IMEI_Destinazione = c.String(),
                        Targa_Destinazione = c.String(),
                        Data = c.DateTime(nullable: false),
                        IdTemplateGiro = c.Int(nullable: false),
                        Prelievi_JSON = c.String(),
                        Lat = c.Decimal(precision: 18, scale: 2),
                        Lng = c.Decimal(precision: 18, scale: 2),
                        Chiuso = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TRASBORDI");
        }
    }
}
