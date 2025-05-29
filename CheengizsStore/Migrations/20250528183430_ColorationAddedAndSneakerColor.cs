using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CheengizsStore.Migrations
{
    /// <inheritdoc />
    public partial class ColorationAddedAndSneakerColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "SneakerColors");

            migrationBuilder.AddColumn<string>(
                name: "Coloration",
                table: "SneakerColors",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SneakerPhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhotoPath = table.Column<string>(type: "text", nullable: false),
                    SneakerColorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SneakerPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SneakerPhoto_SneakerColors_SneakerColorId",
                        column: x => x.SneakerColorId,
                        principalTable: "SneakerColors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SneakerPhoto_SneakerColorId",
                table: "SneakerPhoto",
                column: "SneakerColorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SneakerPhoto");

            migrationBuilder.DropColumn(
                name: "Coloration",
                table: "SneakerColors");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "SneakerColors",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
