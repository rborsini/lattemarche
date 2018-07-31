namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Aggiunta_V_PrelieviLatte : ViewMigration
    {
        public override void Up()
        {
            UpView("V_PrelieviLatte");

        }
        
        public override void Down()
        {
        }
    }
}
