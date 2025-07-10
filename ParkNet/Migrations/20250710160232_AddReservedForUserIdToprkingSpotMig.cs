using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkNet.Migrations
{
    /// <inheritdoc />
    public partial class AddReservedForUserIdToprkingSpotMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReservedForUserId",
                table: "ParkingSpots",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservedForUserId",
                table: "ParkingSpots");
        }
    }
}
