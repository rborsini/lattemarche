using Microsoft.EntityFrameworkCore.Migrations;

namespace LatteMarche.Xamarin.Migrations
{
    public partial class AggiuntoIdCessionarioDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCessionarioDefault",
                table: "Allevamenti",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCessionarioDefault",
                table: "Allevamenti");
        }
    }
}
