namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntoCodiceProduttore_V_Allevamento : ViewMigration
    {
        public override void Up()
        {
            UpView("V_Allevamenti");
        }
        
        public override void Down()
        {

        }
    }
}
