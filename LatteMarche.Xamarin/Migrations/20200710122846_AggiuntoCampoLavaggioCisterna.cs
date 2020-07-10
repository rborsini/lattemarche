using Microsoft.EntityFrameworkCore.Migrations;

namespace LatteMarche.Xamarin.Migrations
{
    public partial class AggiuntoCampoLavaggioCisterna : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LavaggioCisterna",
                table: "Giri",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LavaggioCisterna",
                table: "Giri");
        }
    }
}
