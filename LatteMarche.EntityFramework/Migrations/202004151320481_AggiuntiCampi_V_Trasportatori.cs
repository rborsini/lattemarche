namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntiCampi_V_Trasportatori : ViewMigration
    {
        public override void Up()
        {
            UpView("V_Trasportatori");
        }
        
        public override void Down()
        {

        }
    }
}
