using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Traveling_Platform.Data.Migrations
{
    /// <inheritdoc />
    public partial class MD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProfilePic",
                table: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProfilePic",
                table: "Image",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
