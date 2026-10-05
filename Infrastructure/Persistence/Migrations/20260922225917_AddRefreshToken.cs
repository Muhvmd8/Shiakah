using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthOfDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthOfDate",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
