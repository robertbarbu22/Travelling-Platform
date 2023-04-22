using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Traveling_Platform.Data.Migrations
{
    /// <inheritdoc />
    public partial class jio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Hotels_HotelId",
                table: "Pictures");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "Pictures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Countrytag",
                table: "Pictures",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_Countrytag",
                table: "Pictures",
                column: "Countrytag");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Countries_Countrytag",
                table: "Pictures",
                column: "Countrytag",
                principalTable: "Countries",
                principalColumn: "tag");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Hotels_HotelId",
                table: "Pictures",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "id_hotel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Countries_Countrytag",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Hotels_HotelId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_Countrytag",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Countrytag",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Countries");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Hotels_HotelId",
                table: "Pictures",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "id_hotel",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
