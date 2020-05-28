using Microsoft.EntityFrameworkCore.Migrations;

namespace LatteMarche.Xamarin.Migrations
{
    public partial class AggiuntoIdCessionario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCessionario",
                table: "Prelievi",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCessionario",
                table: "Prelievi");
        }
    }
}
