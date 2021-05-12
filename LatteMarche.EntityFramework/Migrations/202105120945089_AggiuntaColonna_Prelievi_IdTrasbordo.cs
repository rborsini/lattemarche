namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaColonna_Prelievi_IdTrasbordo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PRELIEVO_LATTE", "ID_TRASBORDO", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PRELIEVO_LATTE", "ID_TRASBORDO");
        }
    }
}
