using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWardrobeItemEntityAndAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WardrobeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Attributes_Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Attributes_Color = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Attributes_Fit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Attributes_Style = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Attributes_Season = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Attributes_Pattern = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WardrobeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WardrobeItems_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WardrobeItems_ApplicationUserId",
                table: "WardrobeItems",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WardrobeItems");
        }
    }
}
