namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCampoPasswordNew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UTENTI", "PASSWORD_NEW", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UTENTI", "PASSWORD_NEW");
        }
    }
}
