using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationsConsole.Migrations
{
    public partial class AggiuntaColonna_Autocisterne_Selezionata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AutoCisterne_IdTrasportatore",
                table: "AutoCisterne");

            migrationBuilder.AddColumn<bool>(
                name: "Selezionata",
                table: "AutoCisterne",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AutoCisterne_IdTrasportatore",
                table: "AutoCisterne",
                column: "IdTrasportatore");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AutoCisterne_IdTrasportatore",
                table: "AutoCisterne");

            migrationBuilder.DropColumn(
                name: "Selezionata",
                table: "AutoCisterne");

            migrationBuilder.CreateIndex(
                name: "IX_AutoCisterne_IdTrasportatore",
                table: "AutoCisterne",
                column: "IdTrasportatore",
                unique: true);
        }
    }
}
