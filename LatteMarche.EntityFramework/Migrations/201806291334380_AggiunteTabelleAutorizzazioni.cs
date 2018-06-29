namespace LatteMarche.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiunteTabelleAutorizzazioni : DbMigration
    {
        public override void Up()
        {
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AUTORIZZAZIONI", "IdRuolo", "dbo.RUOLI");
            DropForeignKey("dbo.RUOLI_UTENTE", "Username", "dbo.UTENTI");
            DropForeignKey("dbo.RUOLI_UTENTE", "IdRuolo", "dbo.RUOLI");
            DropForeignKey("dbo.AUTORIZZAZIONI", "Azione", "dbo.AZIONI");
            DropIndex("dbo.RUOLI_UTENTE", new[] { "Username" });
            DropIndex("dbo.RUOLI_UTENTE", new[] { "IdRuolo" });
            DropIndex("dbo.AUTORIZZAZIONI", new[] { "Azione" });
            DropIndex("dbo.AUTORIZZAZIONI", new[] { "IdRuolo" });
            DropTable("dbo.RUOLI_UTENTE");
            DropTable("dbo.RUOLI");
            DropTable("dbo.AZIONI");
            DropTable("dbo.AUTORIZZAZIONI");
        }
    }
}
