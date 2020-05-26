namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaRelazioneAllevamentiUtente : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ANAGRAFE_ALLEVAMENTO", "ID_ACQUIRENTE_DEFAULT");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ANAGRAFE_ALLEVAMENTO", "ID_ACQUIRENTE_DEFAULT", c => c.Int());
        }
    }
}
