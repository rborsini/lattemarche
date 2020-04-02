namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AggiuntaAnalisiLatte : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ANALISI_LATTE",
                c => new
                {
                    CAMPIONE = c.String(nullable: false, maxLength: 128),
                    CODICE_PRODUTTORE = c.String(),
                    NOME_PRODUTTORE = c.String(),
                    ID_PRODUTTORE = c.Int(),
                    ID_ALLEVAMENTO = c.Int(),
                    CODICE_ASL = c.String(),
                    TIPO_LATTE = c.String(),
                    ID_TIPO_LATTE = c.Int(),
                    DATA_RAPPORTO_DI_PROVA = c.DateTime(),
                    DATA_ACCETTAZIONE = c.DateTime(),
                    DATA_PRELIEVO = c.DateTime(),
                })
                .PrimaryKey(t => t.CAMPIONE);

            CreateTable(
                "dbo.ANALISI_LATTE_VALORI",
                c => new
                {
                    ID = c.Long(nullable: false, identity: true),
                    NOME = c.String(),
                    UOM = c.String(),
                    VALORE = c.String(),
                    FUORI_SOGLIA = c.Boolean(nullable: false),
                    Analisi_Id = c.String(maxLength: 128),
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
