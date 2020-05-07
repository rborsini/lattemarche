namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiunteCoordinateAllevamenti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ANAGRAFE_ALLEVAMENTO", "LATITUDINE", c => c.Double());
            AddColumn("dbo.ANAGRAFE_ALLEVAMENTO", "LONGITUDINE", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ANAGRAFE_ALLEVAMENTO", "LONGITUDINE");
            DropColumn("dbo.ANAGRAFE_ALLEVAMENTO", "LATITUDINE");
        }
    }
}
