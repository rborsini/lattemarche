namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntiValoriDefaultAllevamenti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ANAGRAFE_ALLEVAMENTO", "ID_ACQUIRENTE_DEFAULT", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ANAGRAFE_ALLEVAMENTO", "ID_ACQUIRENTE_DEFAULT");
        }
    }
}
