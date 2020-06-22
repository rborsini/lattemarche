using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationsConsole.Migrations
{
    public partial class AggiuntaTabellaAmbienti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ambienti",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Selezionato = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ambienti", x => x.Id);
                });

            migrationBuilder.Sql(@"
                INSERT INTO Ambienti (Nome, Url, Selezionato)
                VALUES
                ('Prod', 'http://lattemarche.azurewebsites.net/', 1),
                ('Test', 'http://robertoborsini.myqnapcloud.com:81', 0),
            ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ambienti");
        }
    }
}
