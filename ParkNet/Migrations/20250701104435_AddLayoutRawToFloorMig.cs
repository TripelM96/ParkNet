using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkNet.Migrations
{
    /// <inheritdoc />
    public partial class AddLayoutRawToFloorMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LayoutRaw",
                table: "Floors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LayoutRaw",
                table: "Floors");
        }
    }
}
