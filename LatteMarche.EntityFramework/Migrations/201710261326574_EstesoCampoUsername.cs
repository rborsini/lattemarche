namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstesoCampoUsername : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UTENTI", "LOGIN", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UTENTI", "LOGIN", c => c.String(maxLength: 8));
        }
    }
}
