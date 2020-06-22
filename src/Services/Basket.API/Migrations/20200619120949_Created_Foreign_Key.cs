using Microsoft.EntityFrameworkCore.Migrations;

namespace Basket.API.Migrations
{
    public partial class Created_Foreign_Key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BasketId",
                table: "Bookings",
                newName: "BasketID");

            migrationBuilder.AlterColumn<int>(
                name: "BasketID",
                table: "Bookings",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BasketID",
                table: "Bookings",
                column: "BasketID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Baskets_BasketID",
                table: "Bookings",
                column: "BasketID",
                principalTable: "Baskets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Baskets_BasketID",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BasketID",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "BasketID",
                table: "Bookings",
                newName: "BasketId");

            migrationBuilder.AlterColumn<int>(
                name: "BasketId",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
