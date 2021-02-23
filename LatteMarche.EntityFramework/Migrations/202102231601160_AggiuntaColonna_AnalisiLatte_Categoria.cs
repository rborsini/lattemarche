namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaColonna_AnalisiLatte_Categoria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ANALISI_LATTE", "CATEGORIA", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ANALISI_LATTE", "CATEGORIA");
        }
    }
}
