namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCampo_NomeProduttore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ANALISI_LATTE", "NOME_PRODUTTORE", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ANALISI_LATTE", "NOME_PRODUTTORE");
        }
    }
}
