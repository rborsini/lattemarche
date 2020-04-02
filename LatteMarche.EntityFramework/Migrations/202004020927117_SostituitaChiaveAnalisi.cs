namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SostituitaChiaveAnalisi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ANALISI_LATTE_VALORI", "Analisi_Id", "dbo.ANALISI_LATTE");
            DropIndex("dbo.ANALISI_LATTE_VALORI", new[] { "Analisi_Id" });
            DropColumn("dbo.ANALISI_LATTE", "CAMPIONE");
            RenameColumn(table: "dbo.ANALISI_LATTE", name: "ID_ANALISI_LATTE", newName: "CAMPIONE");
            DropPrimaryKey("dbo.ANALISI_LATTE");
            AlterColumn("dbo.ANALISI_LATTE", "CAMPIONE", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ANALISI_LATTE", "CAMPIONE", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ANALISI_LATTE_VALORI", "Analisi_Id", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.ANALISI_LATTE", "CAMPIONE");
            CreateIndex("dbo.ANALISI_LATTE_VALORI", "Analisi_Id");
            AddForeignKey("dbo.ANALISI_LATTE_VALORI", "Analisi_Id", "dbo.ANALISI_LATTE", "CAMPIONE");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ANALISI_LATTE_VALORI", "Analisi_Id", "dbo.ANALISI_LATTE");
            DropIndex("dbo.ANALISI_LATTE_VALORI", new[] { "Analisi_Id" });
            DropPrimaryKey("dbo.ANALISI_LATTE");
            AlterColumn("dbo.ANALISI_LATTE_VALORI", "Analisi_Id", c => c.Long());
            AlterColumn("dbo.ANALISI_LATTE", "CAMPIONE", c => c.String());
            AlterColumn("dbo.ANALISI_LATTE", "CAMPIONE", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.ANALISI_LATTE", "ID_ANALISI_LATTE");
            RenameColumn(table: "dbo.ANALISI_LATTE", name: "CAMPIONE", newName: "ID_ANALISI_LATTE");
            AddColumn("dbo.ANALISI_LATTE", "CAMPIONE", c => c.String());
            CreateIndex("dbo.ANALISI_LATTE_VALORI", "Analisi_Id");
            AddForeignKey("dbo.ANALISI_LATTE_VALORI", "Analisi_Id", "dbo.ANALISI_LATTE", "ID_ANALISI_LATTE");
        }
    }
}
