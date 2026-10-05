using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SureMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WardrobeItems_AspNetUsers_ApplicationUserId",
                table: "WardrobeItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WardrobeItems",
                table: "WardrobeItems");

            migrationBuilder.DropIndex(
                name: "IX_WardrobeItems_ApplicationUserId",
                table: "WardrobeItems");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "WardrobeItems");

            migrationBuilder.RenameTable(
                name: "WardrobeItems",
                newName: "WardrobeItem");

            migrationBuilder.AddColumn<int>(
                name: "WardrobeId",
                table: "WardrobeItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WardrobeItem",
                table: "WardrobeItem",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Wardrobe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wardrobe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wardrobe_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WardrobeItem_WardrobeId",
                table: "WardrobeItem",
                column: "WardrobeId");

            migrationBuilder.CreateIndex(
                name: "IX_Wardrobe_ApplicationUserId",
                table: "Wardrobe",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WardrobeItem_Wardrobe_WardrobeId",
                table: "WardrobeItem",
                column: "WardrobeId",
                principalTable: "Wardrobe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WardrobeItem_Wardrobe_WardrobeId",
                table: "WardrobeItem");

            migrationBuilder.DropTable(
                name: "Wardrobe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WardrobeItem",
                table: "WardrobeItem");

            migrationBuilder.DropIndex(
                name: "IX_WardrobeItem_WardrobeId",
                table: "WardrobeItem");

            migrationBuilder.DropColumn(
                name: "WardrobeId",
                table: "WardrobeItem");

            migrationBuilder.RenameTable(
                name: "WardrobeItem",
                newName: "WardrobeItems");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "WardrobeItems",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WardrobeItems",
                table: "WardrobeItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WardrobeItems_ApplicationUserId",
                table: "WardrobeItems",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WardrobeItems_AspNetUsers_ApplicationUserId",
                table: "WardrobeItems",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
