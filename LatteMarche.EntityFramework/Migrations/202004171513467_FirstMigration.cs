namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ANAGRAFE_ACQUIRENTE",
                c => new
                {
                    ID_ACQUIRENTE = c.Int(nullable: false, identity: true),
                    RAG_SOC_ACQUIRENTE = c.String(),
                    PIVA_ACQUIRENTE = c.String(),
                    INDIRIZZO_ACQUIRENTE = c.String(),
                    ID_COMUNE = c.Int(nullable: false),
                    IDSITRA_ACQUIRENTE = c.Int(),
                })
                .PrimaryKey(t => t.ID_ACQUIRENTE);

            CreateTable(
                "dbo.ANAGRAFE_ALLEVAMENTO",
                c => new
                {
                    ID_ALLEVAMENTO = c.Int(nullable: false, identity: true),
                    CODICE_ASL = c.String(),
                    INDIRIZZO_ALLEVAMENTO = c.String(),
                    ID_UTENTE = c.Int(),
                    ID_COMUNE = c.Int(nullable: false),
                    IDSITRA_STABILIMENTO_ALLEVAMENTO = c.Int(),
                    CUAA = c.String(),
                })
                .PrimaryKey(t => t.ID_ALLEVAMENTO);

            CreateTable(
                "dbo.ALLEVAMENTO_X_GIRO",
                c => new
                {
                    ID_GIRO = c.Int(nullable: false),
                    ID_ALLEVAMENTO = c.Int(nullable: false),
                    PRIORITA = c.Int(),
                })
                .PrimaryKey(t => new { t.ID_GIRO, t.ID_ALLEVAMENTO });

            CreateTable(
                "dbo.ANALISI_LATTE",
                c => new
                {
                    CAMPIONE = c.String(nullable: false, maxLength: 128),
                    CODICE_PRODUTTORE = c.String(),
                    NOME_PRODUTTORE = c.String(),
                    ID_PRODUTTORE = c.Int(),
                    ID_ALLEVAMENTO = c.Int(),
                    CODICE_ASL = c.String(),
                    TIPO_LATTE = c.String(),
                    ID_TIPO_LATTE = c.Int(),
                    DATA_RAPPORTO_DI_PROVA = c.DateTime(),
                    DATA_ACCETTAZIONE = c.DateTime(),
                    DATA_PRELIEVO = c.DateTime(),
                })
                .PrimaryKey(t => t.CAMPIONE);

            CreateTable(
                "dbo.ANALISI_LATTE_VALORI",
                c => new
                {
                    ID = c.Long(nullable: false, identity: true),
                    NOME = c.String(),
                    UOM = c.String(),
                    VALORE = c.String(),
                    FUORI_SOGLIA = c.Boolean(nullable: false),
                    Analisi_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ANALISI_LATTE", t => t.Analisi_Id)
                .Index(t => t.Analisi_Id);

            CreateTable(
                "dbo.AUTOCISTERNA",
                c => new
                {
                    ID_VEICOLO = c.Int(nullable: false, identity: true),
                    MARCA = c.String(),
                    MODELLO = c.String(),
                    TARGA_MEZZO = c.String(),
                    ID_TRASPORTATORE = c.Int(),
                    PORTATA = c.Int(),
                    NUMERO_SCOMPARTI = c.Int(),
                })
                .PrimaryKey(t => t.ID_VEICOLO);

            CreateTable(
                "dbo.AUTORIZZAZIONI",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdRuolo = c.Long(nullable: false),
                    Azione = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AZIONI", t => t.Azione)
                .ForeignKey("dbo.RUOLI", t => t.IdRuolo, cascadeDelete: true)
                .Index(t => t.IdRuolo)
                .Index(t => t.Azione);

            CreateTable(
                "dbo.AZIONI",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 100),
                    Type = c.String(maxLength: 10),
                    Controller = c.String(maxLength: 50),
                    Action = c.String(maxLength: 50),
                    ViewItem = c.String(maxLength: 50),
                    Pagina = c.String(maxLength: 50),
                    Nome = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.RUOLI",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Ruolo = c.String(maxLength: 50),
                    Descrizione = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.RUOLI_UTENTE",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdRuolo = c.Long(),
                    Username = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RUOLI", t => t.IdRuolo)
                .ForeignKey("dbo.UTENTI", t => t.Username, cascadeDelete: true)
                .Index(t => t.IdRuolo)
                .Index(t => t.Username);

            CreateTable(
                "dbo.UTENTI",
                c => new
                {
                    ID_UTENTE = c.Int(nullable: false, identity: true),
                    NOME = c.String(),
                    COGNOME = c.String(),
                    PIVA_CF = c.String(),
                    INDIRIZZO = c.String(),
                    LOGIN = c.String(),
                    PASSWORD_NEW = c.String(),
                    ID_PROFILO = c.Int(nullable: false),
                    ABILITATO = c.Boolean(nullable: false),
                    VISIBILE = c.Boolean(nullable: false),
                    RAGIONE_SOCIALE = c.String(),
                    CODICE_ALLEVATORE = c.String(),
                    QUANTITA_LATTE = c.Int(nullable: false),
                    TELEFONO = c.String(),
                    CELLULARE = c.String(),
                    ID_COMUNE = c.Int(),
                    SESSO = c.String(),
                    ID_TIPO_LATTE = c.Int(nullable: false),
                    NUMERO_COMUNICAZIONE = c.String(),
                    NOTE = c.String(),
                })
                .PrimaryKey(t => t.ID_UTENTE);

            CreateTable(
                "dbo.COMUNI",
                c => new
                {
                    ID_COMUNE = c.Int(nullable: false, identity: true),
                    DESCRIZIONE = c.String(),
                    PROVINCIA = c.String(maxLength: 2),
                    CAP = c.String(maxLength: 5),
                })
                .PrimaryKey(t => t.ID_COMUNE);

            CreateTable(
                "dbo.ANAGRAFE_DESTINATARIO",
                c => new
                {
                    ID_DESTINATARIO = c.Int(nullable: false, identity: true),
                    RAG_SOC_DESTINATARIO = c.String(),
                    PIVA_DESTINATARIO = c.String(),
                    INDIRIZZO_DESTINATARIO = c.String(),
                    ID_COMUNE = c.Int(),
                    STABILIMENTO = c.String(),
                    IDSITRA_STABILIMENTO_CASEIFICIO = c.Int(),
                })
                .PrimaryKey(t => t.ID_DESTINATARIO);

            CreateTable(
                "dbo.DISPOSITIVI_MOBILI",
                c => new
                {
                    IMEI = c.String(nullable: false, maxLength: 128),
                    USERNAME = c.String(),
                    ATTIVO = c.Boolean(nullable: false),
                    DATA_REGISTRAZIONE = c.DateTime(nullable: false),
                    DATA_ULTIMO_DOWNLOAD = c.DateTime(),
                    DATA_ULTIMO_UPLOAD = c.DateTime(),
                    VERSIONE_APP = c.String(),
                    LATITUDINE = c.Decimal(precision: 18, scale: 2),
                    LONGITUDINE = c.Decimal(precision: 18, scale: 2),
                    ID_GIRO = c.Int(),
                })
                .PrimaryKey(t => t.IMEI);

            CreateTable(
                "dbo.DOCUMENTI",
                c => new
                {
                    ID_DOCUMENTO = c.Int(nullable: false, identity: true),
                    DESCRIZIONE = c.String(),
                    PATH_DOCUMENTO = c.String(),
                    ID_UTENTE = c.Int(nullable: false),
                    DATA_INSERIMENTO = c.DateTime(),
                })
                .PrimaryKey(t => t.ID_DOCUMENTO);

            CreateTable(
                "dbo.GIRO",
                c => new
                {
                    ID_GIRO = c.Int(nullable: false, identity: true),
                    DENOMINAZIONE = c.String(),
                    CODICE_GIRO = c.String(),
                    ID_TRASPORTATORE = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID_GIRO);

            CreateTable(
                "dbo.LaboratoriAnalisi",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Descrizione = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Logs",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Date = c.DateTime(nullable: false),
                    Thread = c.String(),
                    Level = c.String(),
                    Logger = c.String(),
                    Message = c.String(),
                    Exception = c.String(),
                    Source = c.String(),
                    HostName = c.String(),
                    Identity = c.String(),
                    Duration = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Request = c.String(),
                    Arguments = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LOTTI",
                c => new
                {
                    ID_LOTTO = c.Long(nullable: false, identity: true),
                    CODICE = c.String(),
                    CODICE_SITRA = c.String(),
                    TS = c.DateTime(nullable: false),
                    INVIATO = c.Boolean(nullable: false),
                    ERRORE = c.Boolean(nullable: false),
                    MESSAGGIO = c.String(),
                })
                .PrimaryKey(t => t.ID_LOTTO);

            CreateTable(
                "dbo.PRELIEVO_LATTE",
                c => new
                {
                    ID_PRELIEVO = c.Int(nullable: false, identity: true),
                    ID_ALLEVAMENTO = c.Int(),
                    ID_DESTINATARIO = c.Int(),
                    ID_ACQUIRENTE = c.Int(),
                    ID_TRASPORTATORE = c.Int(),
                    ID_LABANALISI = c.Int(),
                    DATA_PRELIEVO = c.DateTime(),
                    DATA_CONSEGNA = c.DateTime(),
                    DATA_ULTIMA_MUNGITURA = c.DateTime(),
                    QUANTITA = c.Decimal(precision: 18, scale: 2),
                    TEMPERATURA = c.Decimal(precision: 18, scale: 2),
                    NUMERO_MUNGITURE = c.Int(),
                    SCOMPARTO = c.String(),
                    LOTTO_CONSEGNA = c.String(),
                    SERIALE_LAB_ANALISI = c.String(),
                    LastOperation = c.Int(nullable: false),
                    LastChange = c.DateTime(nullable: false),
                    CODICE_SITRA = c.String(),
                })
                .PrimaryKey(t => t.ID_PRELIEVO);

            CreateTable(
                "dbo.TIPO_LATTE",
                c => new
                {
                    ID_TIPO_LATTE = c.Int(nullable: false, identity: true),
                    DESCRIZIONE = c.String(),
                    DESCRIZIONE_BREVE = c.String(maxLength: 5),
                    FATTORE_CONVERSIONE = c.Decimal(nullable: false, precision: 18, scale: 3),
                    FLAG_INVIO_SITRA = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.ID_TIPO_LATTE);

            CreateTable(
                "dbo.PROFILO",
                c => new
                {
                    ID_PROFILO = c.Int(nullable: false, identity: true),
                    DESCRIZIONE_PROFILO = c.String(),
                })
                .PrimaryKey(t => t.ID_PROFILO);

            CreateTable(
                "dbo.UTENTE_X_ACQUIRENTE",
                c => new
                {
                    ID_UTENTE = c.Int(nullable: false, identity: true),
                    ID_ACQUIRENTE = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID_UTENTE);

            CreateTable(
                "dbo.UTENTE_X_DESTINATARIO",
                c => new
                {
                    ID_UTENTE = c.Int(nullable: false, identity: true),
                    ID_DESTINATARIO = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID_UTENTE);

            // V_Allevatori
            Sql(@"
                CREATE VIEW V_Allevatori AS
                SELECT        
	                dbo.ANAGRAFE_ALLEVAMENTO.ID_ALLEVAMENTO, 
	                dbo.ANAGRAFE_ALLEVAMENTO.ID_UTENTE, 
	                dbo.ANAGRAFE_ALLEVAMENTO.INDIRIZZO_ALLEVAMENTO, 
	                dbo.ANAGRAFE_ALLEVAMENTO.ID_COMUNE, 
                    dbo.UTENTI.RAGIONE_SOCIALE, 	
	                dbo.UTENTI.NOME, 
	                dbo.UTENTI.COGNOME, 
	                dbo.UTENTI.ID_TIPO_LATTE,
	                dbo.ANAGRAFE_ALLEVAMENTO.IDSITRA_STABILIMENTO_ALLEVAMENTO, 
	                dbo.ANAGRAFE_ALLEVAMENTO.CODICE_ASL,
	                dbo.COMUNI.PROVINCIA, 
                    dbo.COMUNI.DESCRIZIONE
                FROM            

	                dbo.ANAGRAFE_ALLEVAMENTO 
	
	                INNER JOIN
                    dbo.UTENTI ON dbo.ANAGRAFE_ALLEVAMENTO.ID_UTENTE = dbo.UTENTI.ID_UTENTE 
	
	                INNER JOIN
                    dbo.COMUNI ON dbo.ANAGRAFE_ALLEVAMENTO.ID_COMUNE = dbo.COMUNI.ID_COMUNE  
            ");

            // V_Allevamenti
            Sql(@"
                CREATE VIEW V_Allevamenti AS
                SELECT 
                 allevamenti.ID_ALLEVAMENTO,
                 allevamenti.CODICE_ASL,
                 utenti.CODICE_ALLEVATORE,
                 allevamenti.CUAA,
                 allevamenti.ID_COMUNE,
                 allevamenti.INDIRIZZO_ALLEVAMENTO,
                 allevamenti.ID_UTENTE,
                 utenti.RAGIONE_SOCIALE
                FROM            
                 ANAGRAFE_ALLEVAMENTO as allevamenti

                 left outer join UTENTI as utenti
                 on allevamenti.ID_UTENTE = utenti.ID_UTENTE
            ");

            // V_PrelieviLatte
            Sql(@"
                CREATE VIEW V_PrelieviLatte AS
                SELECT        
                 prelievi.ID_PRELIEVO,
                 prelievi.DATA_PRELIEVO,
                 prelievi.DATA_CONSEGNA,
                 prelievi.QUANTITA,
                 prelievi.TEMPERATURA,
                 prelievi.DATA_ULTIMA_MUNGITURA,
                 prelievi.ID_ALLEVAMENTO,
                 trim(utenti_allevamento.COGNOME) + ' ' + trim(utenti_allevamento.NOME)  as DESCR_ALLEVAMENTO,
                 utenti_allevamento.PIVA_CF as PIVA_ALLEVAMENTO,
                 prelievi.ID_DESTINATARIO,
                 destinatari.RAG_SOC_DESTINATARIO,
                 prelievi.ID_ACQUIRENTE,
                 acquirenti.RAG_SOC_ACQUIRENTE,
                 prelievi.ID_LABANALISI,
                 prelievi.ID_TRASPORTATORE,
                 trasportatori.RAGIONE_SOCIALE as TRASPORTATORE,
                 autocisterne.TARGA_MEZZO,
                 prelievi.NUMERO_MUNGITURE,
                 prelievi.SCOMPARTO,
                 prelievi.LOTTO_CONSEGNA,
                 prelievi.CODICE_SITRA,
                 tipo_latte.FATTORE_CONVERSIONE,
                    tipo_latte.ID_TIPO_LATTE,
                    tipo_latte.DESCRIZIONE AS DESCR_LATTE, 
                    tipo_latte.DESCRIZIONE_BREVE AS SIGLA_LATTE
                FROM            

                 PRELIEVO_LATTE AS prelievi

                 LEFT OUTER JOIN
                 ANAGRAFE_ALLEVAMENTO as allevamenti on prelievi.ID_ALLEVAMENTO = allevamenti.ID_ALLEVAMENTO

                 LEFT OUTER JOIN
                 UTENTI as utenti_allevamento on allevamenti.ID_UTENTE = utenti_allevamento.ID_UTENTE

                 LEFT OUTER JOIN
                 ANAGRAFE_DESTINATARIO as destinatari on prelievi.ID_DESTINATARIO = destinatari.ID_DESTINATARIO

                 LEFT OUTER JOIN
                 ANAGRAFE_ACQUIRENTE as acquirenti on prelievi.ID_ACQUIRENTE = acquirenti.ID_ACQUIRENTE

                 LEFT OUTER JOIN
                 UTENTI as trasportatori on prelievi.ID_TRASPORTATORE = trasportatori.ID_UTENTE

                 LEFT OUTER JOIN
                 AUTOCISTERNA as autocisterne on prelievi.ID_TRASPORTATORE = autocisterne.ID_TRASPORTATORE

                 LEFT OUTER JOIN
                 TIPO_LATTE as tipo_latte on utenti_allevamento.ID_TIPO_LATTE = tipo_latte.ID_TIPO_LATTE
            ");

            // V_Trasportatori
            Sql(@"
                CREATE VIEW V_Trasportatori AS
                SELECT DISTINCT 

                 dbo.UTENTI.NOME, 
                 dbo.UTENTI.COGNOME, 
                 dbo.UTENTI.INDIRIZZO, 
                 dbo.UTENTI.TELEFONO,
                 dbo.UTENTI.CELLULARE, 
                 dbo.UTENTI.ID_UTENTE, 
                 dbo.UTENTI.PIVA_CF,
                 dbo.UTENTI.RAGIONE_SOCIALE,
                 dbo.COMUNI.DESCRIZIONE, 
                 dbo.COMUNI.PROVINCIA

                FROM            

                 dbo.UTENTI 

                 INNER JOIN
                    dbo.COMUNI ON dbo.UTENTI.ID_COMUNE = dbo.COMUNI.ID_COMUNE 

                 INNER JOIN
                    dbo.PROFILO ON dbo.UTENTI.ID_PROFILO = dbo.PROFILO.ID_PROFILO 

                 CROSS JOIN
                    dbo.PROFILO AS PROFILO_1

                WHERE        
                 (dbo.PROFILO.DESCRIZIONE_PROFILO = 'Trasportatore')
            ");

        }

        public override void Down()
        {
            DropForeignKey("dbo.AUTORIZZAZIONI", "IdRuolo", "dbo.RUOLI");
            DropForeignKey("dbo.RUOLI_UTENTE", "Username", "dbo.UTENTI");
            DropForeignKey("dbo.RUOLI_UTENTE", "IdRuolo", "dbo.RUOLI");
            DropForeignKey("dbo.AUTORIZZAZIONI", "Azione", "dbo.AZIONI");
            DropForeignKey("dbo.ANALISI_LATTE_VALORI", "Analisi_Id", "dbo.ANALISI_LATTE");
            DropIndex("dbo.RUOLI_UTENTE", new[] { "Username" });
            DropIndex("dbo.RUOLI_UTENTE", new[] { "IdRuolo" });
            DropIndex("dbo.AUTORIZZAZIONI", new[] { "Azione" });
            DropIndex("dbo.AUTORIZZAZIONI", new[] { "IdRuolo" });
            DropIndex("dbo.ANALISI_LATTE_VALORI", new[] { "Analisi_Id" });
            DropTable("dbo.V_PrelieviLatte");
            DropTable("dbo.V_Allevamenti");
            DropTable("dbo.UTENTE_X_DESTINATARIO");
            DropTable("dbo.UTENTE_X_ACQUIRENTE");
            DropTable("dbo.V_Trasportatori");
            DropTable("dbo.PROFILO");
            DropTable("dbo.TIPO_LATTE");
            DropTable("dbo.PRELIEVO_LATTE");
            DropTable("dbo.LOTTI");
            DropTable("dbo.Logs");
            DropTable("dbo.LaboratoriAnalisi");
            DropTable("dbo.GIRO");
            DropTable("dbo.DOCUMENTI");
            DropTable("dbo.DISPOSITIVI_MOBILI");
            DropTable("dbo.ANAGRAFE_DESTINATARIO");
            DropTable("dbo.COMUNI");
            DropTable("dbo.UTENTI");
            DropTable("dbo.RUOLI_UTENTE");
            DropTable("dbo.RUOLI");
            DropTable("dbo.AZIONI");
            DropTable("dbo.AUTORIZZAZIONI");
            DropTable("dbo.AUTOCISTERNA");
            DropTable("dbo.ANALISI_LATTE_VALORI");
            DropTable("dbo.ANALISI_LATTE");
            DropTable("dbo.V_Allevatori");
            DropTable("dbo.ALLEVAMENTO_X_GIRO");
            DropTable("dbo.ANAGRAFE_ALLEVAMENTO");
            DropTable("dbo.ANAGRAFE_ACQUIRENTE");
        }
    }
}
