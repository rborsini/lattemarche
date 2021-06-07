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
                        Lat = c.Double(),
                        Lng = c.Double(),
                        Chiuso = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PRELIEVO_LATTE", "ID_TRASBORDO", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PRELIEVO_LATTE", "ID_TRASBORDO");
            DropTable("dbo.TRASBORDI");
        }
    }
}
