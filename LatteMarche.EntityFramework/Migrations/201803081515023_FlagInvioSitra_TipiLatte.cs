namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlagInvioSitra_TipiLatte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TIPO_LATTE", "FLAG_INVIO_SITRA", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TIPO_LATTE", "FLAG_INVIO_SITRA");
        }
    }
}
