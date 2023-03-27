using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Traveling_Platform.Data.Migrations
{
    /// <inheritdoc />
    public partial class Mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Hotels_Hotelid_hotel",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_ClientId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_AspNetUsers_ManagerId",
                table: "Hotels");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ClientId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_ManagerId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Hotelid_hotel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Hotelid_hotel",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Reviews",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ClientId",
                table: "Reviews",
                newName: "IX_Reviews_ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Booking",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_ClientId",
                table: "Booking",
                newName: "IX_Booking_ApplicationUserId");

            migrationBuilder.CreateTable(
                name: "ApplicationUserHotel",
                columns: table => new
                {
                    Hotelsid_hotel = table.Column<int>(type: "int", nullable: false),
                    ReceptionistsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserHotel", x => new { x.Hotelsid_hotel, x.ReceptionistsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserHotel_AspNetUsers_ReceptionistsId",
                        column: x => x.ReceptionistsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserHotel_Hotels_Hotelsid_hotel",
                        column: x => x.Hotelsid_hotel,
                        principalTable: "Hotels",
                        principalColumn: "id_hotel",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserHotel_ReceptionistsId",
                table: "ApplicationUserHotel",
                column: "ReceptionistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_ApplicationUserId",
                table: "Booking",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_ApplicationUserId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "ApplicationUserHotel");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Reviews",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews",
                newName: "IX_Reviews_ClientId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Booking",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_ApplicationUserId",
                table: "Booking",
                newName: "IX_Booking_ClientId");

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Hotels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Hotelid_hotel",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_ManagerId",
                table: "Hotels",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Hotelid_hotel",
                table: "AspNetUsers",
                column: "Hotelid_hotel");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Hotels_Hotelid_hotel",
                table: "AspNetUsers",
                column: "Hotelid_hotel",
                principalTable: "Hotels",
                principalColumn: "id_hotel");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_ClientId",
                table: "Booking",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_AspNetUsers_ManagerId",
                table: "Hotels",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ClientId",
                table: "Reviews",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
