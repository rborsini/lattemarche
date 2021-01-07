namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaColonna_PrelievoLatte_IdTipoLatte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PRELIEVO_LATTE", "ID_TIPO_LATTE", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PRELIEVO_LATTE", "ID_TIPO_LATTE");
        }
    }
}
