namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoNullableValoreAnalisi : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ANALISI_LATTE_VALORI", "VALORE", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ANALISI_LATTE_VALORI", "VALORE", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
