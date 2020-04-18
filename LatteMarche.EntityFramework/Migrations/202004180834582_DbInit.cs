namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Configuration;
    using System.Data.Entity.Migrations;

    public partial class DbInit : DbMigration
    {
        private bool IsTestEnv => ConfigurationManager.ConnectionStrings["LatteMarcheDbContext"].ConnectionString.Contains("TEST");

        public override void Up()
        {

            if (!this.IsTestEnv)
                return;

            CreateTable(
                "dbo.ANAGRAFE_ACQUIRENTE",
                c => new
                {
                    ID_ACQUIRENTE = c.Int(nullable: false, identity: true),
                    RAG_SOC_ACQUIRENTE = c.String(),
                    PIVA_ACQUIRENTE = c.String(),
                    INDIRIZZO_ACQUIRENTE = c.String(),
                    ID_COMUNE = c.Int(),
                    IDSITRA_ACQUIRENTE = c.Int(),
                })
                .PrimaryKey(t => t.ID_ACQUIRENTE)
                .ForeignKey("dbo.COMUNI", t => t.ID_COMUNE)
                .Index(t => t.ID_COMUNE);

            CreateTable(
                "dbo.COMUNI",
                c => new
                {
                    ID_COMUNE = c.Int(nullable: false, identity: true),
                    DESCRIZIONE = c.String(),
                    PROVINCIA = c.String(maxLength: 2),
                    CAP = c.String(maxLength: 5),
                    ISTAT6 = c.String(maxLength: 6),
                })
                .PrimaryKey(t => t.ID_COMUNE);

            Sql(Seeder.Comuni_Data());

            CreateTable(
                "dbo.ANAGRAFE_ALLEVAMENTO",
                c => new
                {
                    ID_ALLEVAMENTO = c.Int(nullable: false, identity: true),
                    CODICE_ASL = c.String(),
                    INDIRIZZO_ALLEVAMENTO = c.String(),
                    ID_UTENTE = c.Int(),
                    ID_COMUNE = c.Int(),
                    CUAA = c.String(),
                    IDSITRA_STABILIMENTO_ALLEVAMENTO = c.Int(),
                })
                .PrimaryKey(t => t.ID_ALLEVAMENTO)
                .ForeignKey("dbo.COMUNI", t => t.ID_COMUNE)
                .ForeignKey("dbo.UTENTI", t => t.ID_UTENTE)
                .Index(t => t.ID_UTENTE)
                .Index(t => t.ID_COMUNE);

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
                .PrimaryKey(t => t.ID_UTENTE)
                .ForeignKey("dbo.COMUNI", t => t.ID_COMUNE)
                .Index(t => t.ID_COMUNE);

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
                "dbo.RUOLI",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Ruolo = c.String(maxLength: 50),
                    Descrizione = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ALLEVAMENTO_X_GIRO",
                c => new
                {
                    ID_GIRO = c.Int(nullable: false),
                    ID_ALLEVAMENTO = c.Int(nullable: false),
                    PRIORITA = c.Int(),
                })
                .PrimaryKey(t => new { t.ID_GIRO, t.ID_ALLEVAMENTO })
                .ForeignKey("dbo.ANAGRAFE_ALLEVAMENTO", t => t.ID_ALLEVAMENTO, cascadeDelete: true)
                .ForeignKey("dbo.GIRO", t => t.ID_GIRO, cascadeDelete: true)
                .Index(t => t.ID_GIRO)
                .Index(t => t.ID_ALLEVAMENTO);

            CreateTable(
                "dbo.GIRO",
                c => new
                {
                    ID_GIRO = c.Int(nullable: false, identity: true),
                    DENOMINAZIONE = c.String(),
                    CODICE_GIRO = c.String(),
                    ID_TRASPORTATORE = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID_GIRO)
                .ForeignKey("dbo.UTENTI", t => t.ID_TRASPORTATORE, cascadeDelete: true)
                .Index(t => t.ID_TRASPORTATORE);

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
                .PrimaryKey(t => t.ID_VEICOLO)
                .ForeignKey("dbo.UTENTI", t => t.ID_TRASPORTATORE)
                .Index(t => t.ID_TRASPORTATORE);

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

            Sql(Seeder.TipoLatte_Data());

            CreateTable(
                "dbo.PROFILO",
                c => new
                {
                    ID_PROFILO = c.Int(nullable: false, identity: true),
                    DESCRIZIONE_PROFILO = c.String(),
                })
                .PrimaryKey(t => t.ID_PROFILO);

            Sql(Seeder.Profilo_Data());

            CreateTable(
                "dbo.UTENTE_X_ACQUIRENTE",
                c => new
                {
                    ID_UTENTE = c.Int(nullable: false),
                    ID_ACQUIRENTE = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID_UTENTE)
                .ForeignKey("dbo.UTENTI", t => t.ID_UTENTE)
                .Index(t => t.ID_UTENTE);

            CreateTable(
                "dbo.UTENTE_X_DESTINATARIO",
                c => new
                {
                    ID_UTENTE = c.Int(nullable: false),
                    ID_DESTINATARIO = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID_UTENTE)
                .ForeignKey("dbo.UTENTI", t => t.ID_UTENTE)
                .Index(t => t.ID_UTENTE);


            Sql(Seeder.V_Allevatori_Schema());
            Sql(Seeder.V_Allevamenti_Schema());
            Sql(Seeder.V_PrelieviLatte_Schema());
            Sql(Seeder.V_Trasportatori_Schema());

        }

        public override void Down()
        {
            if (!this.IsTestEnv)
                return;

            DropForeignKey("dbo.UTENTE_X_DESTINATARIO", "ID_UTENTE", "dbo.UTENTI");
            DropForeignKey("dbo.UTENTE_X_ACQUIRENTE", "ID_UTENTE", "dbo.UTENTI");
            DropForeignKey("dbo.AUTORIZZAZIONI", "IdRuolo", "dbo.RUOLI");
            DropForeignKey("dbo.AUTORIZZAZIONI", "Azione", "dbo.AZIONI");
            DropForeignKey("dbo.AUTOCISTERNA", "ID_TRASPORTATORE", "dbo.UTENTI");
            DropForeignKey("dbo.ANALISI_LATTE_VALORI", "Analisi_Id", "dbo.ANALISI_LATTE");
            DropForeignKey("dbo.ALLEVAMENTO_X_GIRO", "ID_GIRO", "dbo.GIRO");
            DropForeignKey("dbo.GIRO", "ID_TRASPORTATORE", "dbo.UTENTI");
            DropForeignKey("dbo.ALLEVAMENTO_X_GIRO", "ID_ALLEVAMENTO", "dbo.ANAGRAFE_ALLEVAMENTO");
            DropForeignKey("dbo.ANAGRAFE_ALLEVAMENTO", "ID_UTENTE", "dbo.UTENTI");
            DropForeignKey("dbo.RUOLI_UTENTE", "Username", "dbo.UTENTI");
            DropForeignKey("dbo.RUOLI_UTENTE", "IdRuolo", "dbo.RUOLI");
            DropForeignKey("dbo.UTENTI", "ID_COMUNE", "dbo.COMUNI");
            DropForeignKey("dbo.ANAGRAFE_ALLEVAMENTO", "ID_COMUNE", "dbo.COMUNI");
            DropForeignKey("dbo.ANAGRAFE_ACQUIRENTE", "ID_COMUNE", "dbo.COMUNI");
            DropIndex("dbo.UTENTE_X_DESTINATARIO", new[] { "ID_UTENTE" });
            DropIndex("dbo.UTENTE_X_ACQUIRENTE", new[] { "ID_UTENTE" });
            DropIndex("dbo.AUTORIZZAZIONI", new[] { "Azione" });
            DropIndex("dbo.AUTORIZZAZIONI", new[] { "IdRuolo" });
            DropIndex("dbo.AUTOCISTERNA", new[] { "ID_TRASPORTATORE" });
            DropIndex("dbo.ANALISI_LATTE_VALORI", new[] { "Analisi_Id" });
            DropIndex("dbo.GIRO", new[] { "ID_TRASPORTATORE" });
            DropIndex("dbo.ALLEVAMENTO_X_GIRO", new[] { "ID_ALLEVAMENTO" });
            DropIndex("dbo.ALLEVAMENTO_X_GIRO", new[] { "ID_GIRO" });
            DropIndex("dbo.RUOLI_UTENTE", new[] { "Username" });
            DropIndex("dbo.RUOLI_UTENTE", new[] { "IdRuolo" });
            DropIndex("dbo.UTENTI", new[] { "ID_COMUNE" });
            DropIndex("dbo.ANAGRAFE_ALLEVAMENTO", new[] { "ID_COMUNE" });
            DropIndex("dbo.ANAGRAFE_ALLEVAMENTO", new[] { "ID_UTENTE" });
            DropIndex("dbo.ANAGRAFE_ACQUIRENTE", new[] { "ID_COMUNE" });
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
            DropTable("dbo.DOCUMENTI");
            DropTable("dbo.DISPOSITIVI_MOBILI");
            DropTable("dbo.ANAGRAFE_DESTINATARIO");
            DropTable("dbo.AZIONI");
            DropTable("dbo.AUTORIZZAZIONI");
            DropTable("dbo.AUTOCISTERNA");
            DropTable("dbo.ANALISI_LATTE_VALORI");
            DropTable("dbo.ANALISI_LATTE");
            DropTable("dbo.V_Allevatori");
            DropTable("dbo.GIRO");
            DropTable("dbo.ALLEVAMENTO_X_GIRO");
            DropTable("dbo.RUOLI");
            DropTable("dbo.RUOLI_UTENTE");
            DropTable("dbo.UTENTI");
            DropTable("dbo.ANAGRAFE_ALLEVAMENTO");
            DropTable("dbo.COMUNI");
            DropTable("dbo.ANAGRAFE_ACQUIRENTE");
        }
    }
}
