namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Aggiunta_V_Trasportatori : DbMigration
    {
        public override void Up()
        {

            string dropQuery = "IF EXISTS (SELECT TOP 1 * FROM sys.views where Name = 'V_Trasportatori') DROP VIEW V_Trasportatori";

            string createQuery = "CREATE VIEW [dbo].[V_Trasportatori] " +
                                    "AS" +
                                    " " +
                                "SELECT DISTINCT " +
                                "" +
                                "" +
                                "    dbo.UTENTI.NOME,  " +
                                "    dbo.UTENTI.COGNOME,  " +
                                "    dbo.UTENTI.INDIRIZZO,  " +
                                "    dbo.UTENTI.TELEFONO, " +
                                "    dbo.UTENTI.CELLULARE,  " +
                                "    dbo.UTENTI.ID_UTENTE,  " +
                                "    dbo.COMUNI.DESCRIZIONE,  " +
                                "    dbo.COMUNI.PROVINCIA " +
                                "" +
                                "FROM " +
                                "" +
                                "" +
                                "    dbo.UTENTI " +
                                "" +
                                "    INNER JOIN " +
                                "    dbo.COMUNI ON dbo.UTENTI.ID_COMUNE = dbo.COMUNI.ID_COMUNE " +
                                "" +
                                "" +
                                "    INNER JOIN " +
                                "    dbo.PROFILO ON dbo.UTENTI.ID_PROFILO = dbo.PROFILO.ID_PROFILO " +
                                "" +
                                "" +
                                "    CROSS JOIN " +
                                "    dbo.PROFILO AS PROFILO_1 " +
                                "" +
                                "WHERE " +
                                "    (dbo.PROFILO.DESCRIZIONE_PROFILO = 'Trasportatore')";

            Sql(dropQuery);
            Sql(createQuery);
        }

        public override void Down()
        {
            string dropQuery = "IF EXISTS (SELECT TOP 1 * FROM sys.views where Name = 'V_Trasportatori') DROP VIEW V_Trasportatori";
            Sql(dropQuery);
        }
    }
}
