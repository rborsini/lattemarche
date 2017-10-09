namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Aggiunta_V_Prelievi_Latte : DbMigration
    {
        public override void Up()
        {
            string dropQuery = "IF EXISTS (SELECT TOP 1 * FROM sys.views where Name = 'V_Prelievi_Latte') DROP VIEW V_Prelievi_Latte";

            string createQuery = "CREATE VIEW [dbo].[V_Prelievi_Latte] " +
                                    "AS" +
                                    " " +
                                    "SELECT " +
                                    "    dbo.PRELIEVO_LATTE.DATA_PRELIEVO, " +
                                    "    dbo.PRELIEVO_LATTE.DATA_ULTIMA_MUNGITURA, " +
                                    "    dbo.PRELIEVO_LATTE.NUMERO_MUNGITURE, " +
                                    "    dbo.PRELIEVO_LATTE.QUANTITA, " +
                                    "    dbo.PRELIEVO_LATTE.TEMPERATURA," +
                                    "    dbo.PRELIEVO_LATTE.ID_TRASPORTATORE," +
                                    "    UTENTI_TRASPORTATORI.COGNOME AS TrasportatoreCognome, " +
                                    "    dbo.PRELIEVO_LATTE.ID_ACQUIRENTE, " +
                                    "    UTENTI_DESTINATARI.RAGIONE_SOCIALE AS DestinatarioRagSoc, " +
                                    "    dbo.PRELIEVO_LATTE.DATA_CONSEGNA, " +
                                    "    dbo.PRELIEVO_LATTE.SCOMPARTO, " +
                                    "    dbo.PRELIEVO_LATTE.LOTTO_CONSEGNA, " +
                                    "    dbo.PRELIEVO_LATTE.ID_ALLEVAMENTO, " +
                                    "    dbo.PRELIEVO_LATTE.SERIALE_LAB_ANALISI, " +
                                    "    dbo.PRELIEVO_LATTE.ID_DESTINATARIO, " +
                                    "    dbo.PRELIEVO_LATTE.ID_PRELIEVO, " +
                                    "    dbo.PRELIEVO_LATTE.ID_LABANALISI, " +
                                    "    dbo.ANAGRAFE_ACQUIRENTE.RAG_SOC_ACQUIRENTE AS AcquirenteRagSoc, " +
                                    "    dbo.LaboratoriAnalisi.Descrizione AS LabAnalisi " +
                                    "FROM " +
                                    "" +
                                    "" +
                                    "    dbo.UTENTI AS UTENTI_TRASPORTATORI " +
                                    "" +
                                    "    INNER JOIN " +
                                    "    dbo.PRELIEVO_LATTE ON UTENTI_TRASPORTATORI.ID_UTENTE = dbo.PRELIEVO_LATTE.ID_TRASPORTATORE " +
                                    "" +
                                    "" +
                                    "    INNER JOIN " +
                                    "    dbo.UTENTI AS UTENTI_DESTINATARI ON dbo.PRELIEVO_LATTE.ID_DESTINATARIO = UTENTI_DESTINATARI.ID_UTENTE " +
                                    "" +
                                    "" +
                                    "    INNER JOIN " +
                                    "    dbo.ANAGRAFE_ACQUIRENTE ON dbo.PRELIEVO_LATTE.ID_ACQUIRENTE = dbo.ANAGRAFE_ACQUIRENTE.ID_ACQUIRENTE " +
                                    "" +
                                    "" +
                                    "    INNER JOIN " +
                                    "    dbo.LaboratoriAnalisi ON dbo.PRELIEVO_LATTE.ID_LABANALISI = dbo.LaboratoriAnalisi.Id";


            Sql(dropQuery);
            Sql(createQuery);

        }

        public override void Down()
        {
            string dropQuery = "IF EXISTS (SELECT TOP 1 * FROM sys.views where Name = 'V_Prelievi_Latte') DROP VIEW V_Prelievi_Latte";
            Sql(dropQuery);
        }
    }
}
