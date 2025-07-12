using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkNet.Migrations
{
    /// <inheritdoc />
    public partial class AddReservedForUserMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReservedForUserId",
                table: "ParkingSpots",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpots_ReservedForUserId",
                table: "ParkingSpots",
                column: "ReservedForUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpots_AspNetUsers_ReservedForUserId",
                table: "ParkingSpots",
                column: "ReservedForUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpots_AspNetUsers_ReservedForUserId",
                table: "ParkingSpots");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSpots_ReservedForUserId",
                table: "ParkingSpots");

            migrationBuilder.AlterColumn<string>(
                name: "ReservedForUserId",
                table: "ParkingSpots",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
