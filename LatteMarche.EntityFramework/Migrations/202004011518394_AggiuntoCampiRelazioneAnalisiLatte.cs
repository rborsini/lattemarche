namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCampiRelazioneAnalisiLatte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ANALISI_LATTE", "CODICE_PRODUTTORE", c => c.String());
            AddColumn("dbo.ANALISI_LATTE", "ID_PRODUTTORE", c => c.Int());
            AddColumn("dbo.ANALISI_LATTE", "TIPO_LATTE", c => c.String());
            AddColumn("dbo.ANALISI_LATTE", "ID_TIPO_LATTE", c => c.Int());
            AlterColumn("dbo.ANALISI_LATTE", "ID_ALLEVAMENTO", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ANALISI_LATTE", "ID_ALLEVAMENTO", c => c.Int(nullable: false));
            DropColumn("dbo.ANALISI_LATTE", "ID_TIPO_LATTE");
            DropColumn("dbo.ANALISI_LATTE", "TIPO_LATTE");
            DropColumn("dbo.ANALISI_LATTE", "ID_PRODUTTORE");
            DropColumn("dbo.ANALISI_LATTE", "CODICE_PRODUTTORE");
        }
    }
}
