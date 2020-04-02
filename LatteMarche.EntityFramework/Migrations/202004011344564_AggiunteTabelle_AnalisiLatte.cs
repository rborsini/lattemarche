namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiunteTabelle_AnalisiLatte : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ANALISI_LATTE",
                c => new
                    {
                        ID_ANALISI_LATTE = c.Long(nullable: false, identity: true),
                        CODICE_ASL = c.String(),
                        CAMPIONE = c.String(),
                        DATA_RAPPORTO_DI_PROVA = c.DateTime(),
                        DATA_ACCETTAZIONE = c.DateTime(),
                        DATA_PRELIEVO = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID_ANALISI_LATTE);
            
            CreateTable(
                "dbo.ANALISI_LATTE_VALORI",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        NOME = c.String(),
                        UOM = c.String(),
                        VALORE = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FUORI_SOGLIA = c.Boolean(nullable: false),
                        Analisi_Id = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ANALISI_LATTE", t => t.Analisi_Id)
                .Index(t => t.Analisi_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ANALISI_LATTE_VALORI", "Analisi_Id", "dbo.ANALISI_LATTE");
            DropIndex("dbo.ANALISI_LATTE_VALORI", new[] { "Analisi_Id" });
            DropTable("dbo.ANALISI_LATTE_VALORI");
            DropTable("dbo.ANALISI_LATTE");
        }
    }
}
