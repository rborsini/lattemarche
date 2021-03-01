using Microsoft.EntityFrameworkCore.Migrations;

namespace LatteMarche.Xamarin.Migrations
{
    public partial class AggiuntaColonna_Prelievo_Trasbordo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Trasbordo",
                table: "Prelievi",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Trasbordo",
                table: "Prelievi");
        }
    }
}
