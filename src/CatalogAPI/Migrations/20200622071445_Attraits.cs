using Microsoft.EntityFrameworkCore.Migrations;

namespace CatalogAPI.Migrations
{
    public partial class Attraits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attraits",
                table: "Actors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attraits",
                table: "Actors");
        }
    }
}
