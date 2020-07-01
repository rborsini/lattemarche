namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RinominazioneTabellaCessionari : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Cessionario", newName: "ANAGRAFE_CESSIONARIO");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ANAGRAFE_CESSIONARIO", newName: "Cessionario");
        }
    }
}
