namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaColonna_Tenant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UTENTI", "TENANT", c => c.String(maxLength: 50));
            AddColumn("dbo.DISPOSITIVI_MOBILI", "TENANT", c => c.String(maxLength: 50));

            Sql("UPDATE dbo.UTENTI SET TENANT = 'Cooperlat'");
            Sql("UPDATE dbo.DISPOSITIVI_MOBILI SET TENANT = 'Cooperlat'");

        }
        
        public override void Down()
        {
            DropColumn("dbo.DISPOSITIVI_MOBILI", "TENANT");
            DropColumn("dbo.UTENTI", "TENANT");
        }
    }
}
