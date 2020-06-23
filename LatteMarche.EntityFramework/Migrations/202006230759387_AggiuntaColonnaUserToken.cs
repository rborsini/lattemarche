namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaColonnaUserToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UTENTI", "TOKEN", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UTENTI", "TOKEN");
        }
    }
}
