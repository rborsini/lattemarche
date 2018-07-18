namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCampo_CUAA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ANAGRAFE_ALLEVAMENTO", "CUAA", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ANAGRAFE_ALLEVAMENTO", "CUAA");
        }
    }
}
