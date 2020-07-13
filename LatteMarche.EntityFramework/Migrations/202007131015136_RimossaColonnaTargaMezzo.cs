namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using WeCode.EntityFramework;

    public partial class RimossaColonnaTargaMezzo : ViewMigration
    {
        public override void Up()
        {
            UpView("LatteMarche.EntityFramework", "V_PrelieviLatte");
        }
        
        public override void Down()
        {
        }
    }
}
