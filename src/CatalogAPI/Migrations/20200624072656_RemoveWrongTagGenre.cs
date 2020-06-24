using Microsoft.EntityFrameworkCore.Migrations;

namespace CatalogAPI.Migrations
{
    public partial class RemoveWrongTagGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Genres",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Actors",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Genres",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Actors",
                newName: "ID");
        }
    }
}
