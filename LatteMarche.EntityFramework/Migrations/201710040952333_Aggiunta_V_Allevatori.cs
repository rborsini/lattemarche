namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Aggiunta_V_Allevatori : DbMigration
    {
        public override void Up()
        {

            string dropQuery =      "IF EXISTS (SELECT TOP 1 * FROM sys.views where Name = 'V_Allevatori') DROP VIEW V_Allevatori";
                                
            string createQuery =    "CREATE VIEW [dbo].[V_Allevatori] " +
                                    "AS" +
                                    " " +
                                    "SELECT " +
                                    "    dbo.ANAGRAFE_ALLEVAMENTO.ID_ALLEVAMENTO, " +
	                                "    dbo.ANAGRAFE_ALLEVAMENTO.ID_UTENTE, " +
	                                "    dbo.ANAGRAFE_ALLEVAMENTO.INDIRIZZO_ALLEVAMENTO, " +
	                                "    dbo.ANAGRAFE_ALLEVAMENTO.ID_COMUNE, " +
                                    "    dbo.UTENTI.RAGIONE_SOCIALE, " +
	                                "    dbo.ANAGRAFE_ALLEVAMENTO.IDSITRA_STABILIMENTO_ALLEVAMENTO, " +
	                                "    dbo.ANAGRAFE_ALLEVAMENTO.CODICE_ASL, dbo.COMUNI.PROVINCIA, " +
                                    "    dbo.COMUNI.DESCRIZIONE " +
                                    "FROM " +
                                    "" +
                                    "" + 
                                    "    dbo.ANAGRAFE_ALLEVAMENTO " +
                                    "" +
                                    "    INNER JOIN " +
                                    "    dbo.UTENTI ON dbo.ANAGRAFE_ALLEVAMENTO.ID_UTENTE = dbo.UTENTI.ID_UTENTE " +
                                    "" +
                                    "" +
                                    "    INNER JOIN " +
                                    "    dbo.COMUNI ON dbo.ANAGRAFE_ALLEVAMENTO.ID_COMUNE = dbo.COMUNI.ID_COMUNE";

            Sql(dropQuery);
            Sql(createQuery);
        }
        
        public override void Down()
        {
            string dropQuery = "IF EXISTS (SELECT TOP 1 * FROM sys.views where Name = 'V_Allevatori') DROP VIEW V_Allevatori";
            Sql(dropQuery);
        }
    }
}
