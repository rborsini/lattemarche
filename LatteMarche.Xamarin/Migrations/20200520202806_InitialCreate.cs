using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LatteMarche.Xamarin.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Acquirenti",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        RagioneSociale = table.Column<string>(nullable: true),
            //        P_IVA = table.Column<string>(nullable: true),
            //        Indirizzo = table.Column<string>(nullable: true),
            //        CAP = table.Column<string>(nullable: true),
            //        Comune = table.Column<string>(nullable: true),
            //        Provincia = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Acquirenti", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Destinatari",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        RagioneSociale = table.Column<string>(nullable: true),
            //        Indirizzo = table.Column<string>(nullable: true),
            //        CAP = table.Column<string>(nullable: true),
            //        Comune = table.Column<string>(nullable: true),
            //        Provincia = table.Column<string>(nullable: true),
            //        P_IVA = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Destinatari", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Giri",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        IdTemplateGiro = table.Column<int>(nullable: true),
            //        Titolo = table.Column<string>(nullable: true),
            //        CodiceLotto = table.Column<string>(nullable: true),
            //        DataCreazione = table.Column<DateTime>(nullable: false),
            //        DataConsegna = table.Column<DateTime>(nullable: true),
            //        DataUpload = table.Column<DateTime>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Giri", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Sincronizzazioni",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        Timestamp = table.Column<DateTime>(nullable: false),
            //        Tipo = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Sincronizzazioni", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Stampanti",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        Nome = table.Column<string>(nullable: true),
            //        MacAddress = table.Column<string>(nullable: true),
            //        Selezionata = table.Column<bool>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Stampanti", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TipiLatte",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        Codice = table.Column<string>(nullable: true),
            //        Descrizione = table.Column<string>(nullable: true),
            //        FattoreConversione = table.Column<decimal>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TipiLatte", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Trasportatori",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        RagioneSociale = table.Column<string>(nullable: true),
            //        Indirizzo = table.Column<string>(nullable: true),
            //        P_IVA = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Trasportatori", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Prelievi",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(nullable: false),
            //        Titolo = table.Column<string>(nullable: true),
            //        DataPrelievo = table.Column<DateTime>(nullable: true),
            //        Scomparto = table.Column<string>(nullable: true),
            //        Quantita_kg = table.Column<decimal>(nullable: true),
            //        Quantita_lt = table.Column<decimal>(nullable: true),
            //        Temperatura = table.Column<decimal>(nullable: true),
            //        NumeroMungiture = table.Column<int>(nullable: true),
            //        DataConsegna = table.Column<DateTime>(nullable: true),
            //        DataUltimaMungitura = table.Column<DateTime>(nullable: true),
            //        IdGiro = table.Column<int>(nullable: true),
            //        IdAllevamento = table.Column<int>(nullable: true),
            //        IdAcquirente = table.Column<int>(nullable: true),
            //        IdDestinatario = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Prelievi", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Prelievi_Giri_IdGiro",
            //            column: x => x.IdGiro,
            //            principalTable: "Giri",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AutoCisterne",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        Marca = table.Column<string>(nullable: true),
            //        Modello = table.Column<string>(nullable: true),
            //        Targa = table.Column<string>(nullable: true),
            //        Portata = table.Column<int>(nullable: false),
            //        NumScomparti = table.Column<int>(nullable: false),
            //        IdTrasportatore = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AutoCisterne", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AutoCisterne_Trasportatori_IdTrasportatore",
            //            column: x => x.IdTrasportatore,
            //            principalTable: "Trasportatori",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TemplateGiri",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        Codice = table.Column<string>(nullable: true),
            //        Descrizione = table.Column<string>(nullable: true),
            //        IdTrasportatore = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TemplateGiri", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TemplateGiri_Trasportatori_IdTrasportatore",
            //            column: x => x.IdTrasportatore,
            //            principalTable: "Trasportatori",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Allevamenti",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        IdAllevamento = table.Column<int>(nullable: false),
            //        RagioneSociale = table.Column<string>(nullable: true),
            //        P_IVA = table.Column<string>(nullable: true),
            //        CAP = table.Column<string>(nullable: true),
            //        Indirizzo = table.Column<string>(nullable: true),
            //        Comune = table.Column<string>(nullable: true),
            //        Provincia = table.Column<string>(nullable: true),
            //        IdTemplateGiro = table.Column<int>(nullable: true),
            //        Priorita = table.Column<int>(nullable: false),
            //        IdTipoLatte = table.Column<int>(nullable: true),
            //        Latitudine = table.Column<double>(nullable: true),
            //        Longitudine = table.Column<double>(nullable: true),
            //        IdAcquirenteDefault = table.Column<int>(nullable: true),
            //        IdDestinatarioDefault = table.Column<int>(nullable: true),
            //        Quantita_Min = table.Column<decimal>(nullable: true),
            //        Quantita_Max = table.Column<decimal>(nullable: true),
            //        Temperatura_Min = table.Column<decimal>(nullable: true),
            //        Temperatura_Max = table.Column<decimal>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Allevamenti", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Allevamenti_TemplateGiri_IdTemplateGiro",
            //            column: x => x.IdTemplateGiro,
            //            principalTable: "TemplateGiri",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Allevamenti_TipiLatte_IdTipoLatte",
            //            column: x => x.IdTipoLatte,
            //            principalTable: "TipiLatte",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Allevamenti_IdTemplateGiro",
            //    table: "Allevamenti",
            //    column: "IdTemplateGiro");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Allevamenti_IdTipoLatte",
            //    table: "Allevamenti",
            //    column: "IdTipoLatte");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AutoCisterne_IdTrasportatore",
            //    table: "AutoCisterne",
            //    column: "IdTrasportatore",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Prelievi_IdGiro",
            //    table: "Prelievi",
            //    column: "IdGiro");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TemplateGiri_IdTrasportatore",
            //    table: "TemplateGiri",
            //    column: "IdTrasportatore");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acquirenti");

            //migrationBuilder.DropTable(
            //    name: "Allevamenti");

            //migrationBuilder.DropTable(
            //    name: "AutoCisterne");

            //migrationBuilder.DropTable(
            //    name: "Destinatari");

            //migrationBuilder.DropTable(
            //    name: "Prelievi");

            //migrationBuilder.DropTable(
            //    name: "Sincronizzazioni");

            //migrationBuilder.DropTable(
            //    name: "Stampanti");

            //migrationBuilder.DropTable(
            //    name: "TemplateGiri");

            //migrationBuilder.DropTable(
            //    name: "TipiLatte");

            //migrationBuilder.DropTable(
            //    name: "Giri");

            //migrationBuilder.DropTable(
            //    name: "Trasportatori");
        }
    }
}
