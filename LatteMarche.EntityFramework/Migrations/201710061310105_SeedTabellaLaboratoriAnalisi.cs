namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedTabellaLaboratoriAnalisi : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO LaboratoriAnalisi (Descrizione) VALUES ('NON DEFINITO')");
            Sql("INSERT INTO LaboratoriAnalisi (Descrizione) VALUES ('ASSAM')");
        }

        public override void Down()
        {
            Sql("DELETE FROM LaboratoriAnalisi WHERE 1=1");
        }
    }
}
