using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Traveling_Platform.Data.Migrations
{
    /// <inheritdoc />
    public partial class M1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelRooms");

            migrationBuilder.AddColumn<int>(
                name: "Hotelid_hotel",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdHotel",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PricePerNight",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Hotelid_hotel",
                table: "Rooms",
                column: "Hotelid_hotel");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Hotels_Hotelid_hotel",
                table: "Rooms",
                column: "Hotelid_hotel",
                principalTable: "Hotels",
                principalColumn: "id_hotel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Hotels_Hotelid_hotel",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_Hotelid_hotel",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Hotelid_hotel",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IdHotel",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PricePerNight",
                table: "Rooms");

            migrationBuilder.CreateTable(
                name: "HotelRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRoom = table.Column<int>(type: "int", nullable: false),
                    IdHotel = table.Column<int>(type: "int", nullable: false),
                    NumberOfRooms = table.Column<int>(type: "int", nullable: false),
                    PricePerNight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRooms", x => new { x.Id, x.IdRoom, x.IdHotel });
                    table.ForeignKey(
                        name: "FK_HotelRooms_Hotels_IdHotel",
                        column: x => x.IdHotel,
                        principalTable: "Hotels",
                        principalColumn: "id_hotel",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelRooms_Rooms_IdRoom",
                        column: x => x.IdRoom,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelRooms_IdHotel",
                table: "HotelRooms",
                column: "IdHotel");

            migrationBuilder.CreateIndex(
                name: "IX_HotelRooms_IdRoom",
                table: "HotelRooms",
                column: "IdRoom");
        }
    }
}
