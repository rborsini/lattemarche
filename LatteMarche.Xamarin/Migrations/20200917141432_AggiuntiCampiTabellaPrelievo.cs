using Microsoft.EntityFrameworkCore.Migrations;

namespace LatteMarche.Xamarin.Migrations
{
    public partial class AggiuntiCampiTabellaPrelievo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdAutocisterna",
                table: "Prelievi",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Lat",
                table: "Prelievi",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Lng",
                table: "Prelievi",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdAutocisterna",
                table: "Prelievi");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Prelievi");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "Prelievi");
        }
    }
}
