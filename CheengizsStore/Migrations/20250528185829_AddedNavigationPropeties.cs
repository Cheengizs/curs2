using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheengizsStore.Migrations
{
    /// <inheritdoc />
    public partial class AddedNavigationPropeties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SneakerPhoto_SneakerColors_SneakerColorId",
                table: "SneakerPhoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SneakerPhoto",
                table: "SneakerPhoto");

            migrationBuilder.RenameTable(
                name: "SneakerPhoto",
                newName: "SneakerPhotos");

            migrationBuilder.RenameIndex(
                name: "IX_SneakerPhoto_SneakerColorId",
                table: "SneakerPhotos",
                newName: "IX_SneakerPhotos_SneakerColorId");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoPath",
                table: "SneakerPhotos",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SneakerPhotos",
                table: "SneakerPhotos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SneakerPhotos_SneakerColors_SneakerColorId",
                table: "SneakerPhotos",
                column: "SneakerColorId",
                principalTable: "SneakerColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SneakerPhotos_SneakerColors_SneakerColorId",
                table: "SneakerPhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SneakerPhotos",
                table: "SneakerPhotos");

            migrationBuilder.RenameTable(
                name: "SneakerPhotos",
                newName: "SneakerPhoto");

            migrationBuilder.RenameIndex(
                name: "IX_SneakerPhotos_SneakerColorId",
                table: "SneakerPhoto",
                newName: "IX_SneakerPhoto_SneakerColorId");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoPath",
                table: "SneakerPhoto",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SneakerPhoto",
                table: "SneakerPhoto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SneakerPhoto_SneakerColors_SneakerColorId",
                table: "SneakerPhoto",
                column: "SneakerColorId",
                principalTable: "SneakerColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
