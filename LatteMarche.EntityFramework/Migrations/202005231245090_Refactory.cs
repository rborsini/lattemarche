namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refactory : DbMigration
    {
        public override void Up()
        {
            //Drop("dbo.V_Allevatori");
            //DropTable("dbo.V_Trasportatori");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.V_Trasportatori",
            //    c => new
            //        {
            //            ID_UTENTE = c.Int(nullable: false, identity: true),
            //            COGNOME = c.String(),
            //            NOME = c.String(),
            //            INDIRIZZO = c.String(),
            //            DESCRIZIONE = c.String(),
            //            PROVINCIA = c.String(),
            //            TELEFONO = c.String(),
            //            CELLULARE = c.String(),
            //            PIVA_CF = c.String(),
            //            RAGIONE_SOCIALE = c.String(),
            //        })
            //    .PrimaryKey(t => t.ID_UTENTE);
            
            //CreateTable(
            //    "dbo.V_Allevatori",
            //    c => new
            //        {
            //            ID_ALLEVAMENTO = c.Int(nullable: false, identity: true),
            //            CODICE_ASL = c.String(),
            //            INDIRIZZO_ALLEVAMENTO = c.String(),
            //            NOME = c.String(),
            //            COGNOME = c.String(),
            //            RAGIONE_SOCIALE = c.String(),
            //            DESCRIZIONE = c.String(),
            //            ID_TIPO_LATTE = c.Int(nullable: false),
            //            PROVINCIA = c.String(),
            //            ID_UTENTE = c.Int(nullable: false),
            //            ID_COMUNE = c.Int(nullable: false),
            //            IDSITRA_STABILIMENTO_ALLEVAMENTO = c.Int(),
            //        })
            //    .PrimaryKey(t => t.ID_ALLEVAMENTO);
            
        }
    }
}
