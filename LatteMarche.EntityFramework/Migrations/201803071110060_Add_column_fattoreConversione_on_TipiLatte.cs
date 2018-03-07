namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_column_fattoreConversione_on_TipiLatte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TIPO_LATTE", "FATTORE_CONVERSIONE", c => c.Decimal(nullable: false, precision: 18, scale: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TIPO_LATTE", "FATTORE_CONVERSIONE");
        }
    }
}
