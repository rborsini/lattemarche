namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Eliminazione_V_Prelievi_Latte : DbMigration
    {
        public override void Up()
        {
            string dropQuery = "IF EXISTS (SELECT TOP 1 * FROM sys.views where Name = 'V_Prelievi_Latte') DROP VIEW V_Prelievi_Latte";
            Sql(dropQuery);
        }
        
        public override void Down()
        {
            string dropQuery = "IF EXISTS (SELECT TOP 1 * FROM sys.views where Name = 'V_Prelievi_Latte') DROP VIEW V_Prelievi_Latte";
            Sql(dropQuery);
        }
    }
}