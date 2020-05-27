using Microsoft.EntityFrameworkCore.Migrations;

namespace LatteMarche.Xamarin.Migrations
{
    public partial class AggiuntaTabellaCessionari : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cessionari",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RagioneSociale = table.Column<string>(nullable: true),
                    P_IVA = table.Column<string>(nullable: true),
                    Indirizzo = table.Column<string>(nullable: true),
                    CAP = table.Column<string>(nullable: true),
                    Comune = table.Column<string>(nullable: true),
                    Provincia = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cessionari", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cessionari");
        }
    }
}
