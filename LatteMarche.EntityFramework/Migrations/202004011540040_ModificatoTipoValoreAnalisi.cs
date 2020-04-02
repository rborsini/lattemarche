namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificatoTipoValoreAnalisi : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ANALISI_LATTE_VALORI", "VALORE", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ANALISI_LATTE_VALORI", "VALORE", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
