namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaTabellaLaboratoriAnalisi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LaboratoriAnalisi",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LaboratoriAnalisi");
        }
    }
}
