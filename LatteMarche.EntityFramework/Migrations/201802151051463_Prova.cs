namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Prova : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PRELIEVO_LATTE", "ID_ALLEVAMENTO", c => c.Int());
            AlterColumn("dbo.PRELIEVO_LATTE", "ID_DESTINATARIO", c => c.Int());
            AlterColumn("dbo.PRELIEVO_LATTE", "ID_ACQUIRENTE", c => c.Int());
            AlterColumn("dbo.PRELIEVO_LATTE", "ID_TRASPORTATORE", c => c.Int());
            AlterColumn("dbo.PRELIEVO_LATTE", "ID_LABANALISI", c => c.Int());
            AlterColumn("dbo.PRELIEVO_LATTE", "DATA_PRELIEVO", c => c.DateTime());
            AlterColumn("dbo.PRELIEVO_LATTE", "DATA_CONSEGNA", c => c.DateTime());
            AlterColumn("dbo.PRELIEVO_LATTE", "DATA_ULTIMA_MUNGITURA", c => c.DateTime());
            AlterColumn("dbo.PRELIEVO_LATTE", "QUANTITA", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.PRELIEVO_LATTE", "TEMPERATURA", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.PRELIEVO_LATTE", "NUMERO_MUNGITURE", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PRELIEVO_LATTE", "NUMERO_MUNGITURE", c => c.Int(nullable: false));
            AlterColumn("dbo.PRELIEVO_LATTE", "TEMPERATURA", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.PRELIEVO_LATTE", "QUANTITA", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.PRELIEVO_LATTE", "DATA_ULTIMA_MUNGITURA", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PRELIEVO_LATTE", "DATA_CONSEGNA", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PRELIEVO_LATTE", "DATA_PRELIEVO", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PRELIEVO_LATTE", "ID_LABANALISI", c => c.Int(nullable: false));
            AlterColumn("dbo.PRELIEVO_LATTE", "ID_TRASPORTATORE", c => c.Int(nullable: false));
            AlterColumn("dbo.PRELIEVO_LATTE", "ID_ACQUIRENTE", c => c.Int(nullable: false));
            AlterColumn("dbo.PRELIEVO_LATTE", "ID_DESTINATARIO", c => c.Int(nullable: false));
            AlterColumn("dbo.PRELIEVO_LATTE", "ID_ALLEVAMENTO", c => c.Int(nullable: false));
        }
    }
}
