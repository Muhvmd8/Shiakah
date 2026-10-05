using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOutfitAiResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutfitResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Top = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Bottom = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Footwear = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Accessories = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RecommendedImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SearchQuery = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StylingTips = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutfitResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutfitResult_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutfitResult_ApplicationUserId",
                table: "OutfitResult",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutfitResult");
        }
    }
}
