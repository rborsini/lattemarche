namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCampoIdAllevamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ANALISI_LATTE", "ID_ALLEVAMENTO", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ANALISI_LATTE", "ID_ALLEVAMENTO");
        }
    }
}
