using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CheengizsStore.Migrations
{
    /// <inheritdoc />
    public partial class NEWSTRUCTURE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_SneakerToSales_SneakerToSaleId",
                table: "Carts");

            migrationBuilder.DropTable(
                name: "SneakerToSales");

            migrationBuilder.RenameColumn(
                name: "SneakerToSaleId",
                table: "Carts",
                newName: "SneakerProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_SneakerToSaleId",
                table: "Carts",
                newName: "IX_Carts_SneakerProductId");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "SneakerColors",
                type: "numeric(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_SneakerProducts_SneakerProductId",
                table: "Carts",
                column: "SneakerProductId",
                principalTable: "SneakerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_SneakerProducts_SneakerProductId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "SneakerColors");

            migrationBuilder.RenameColumn(
                name: "SneakerProductId",
                table: "Carts",
                newName: "SneakerToSaleId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_SneakerProductId",
                table: "Carts",
                newName: "IX_Carts_SneakerToSaleId");

            migrationBuilder.CreateTable(
                name: "SneakerToSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SneakerProductId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SneakerToSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SneakerToSales_SneakerProducts_SneakerProductId",
                        column: x => x.SneakerProductId,
                        principalTable: "SneakerProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SneakerToSales_SneakerProductId",
                table: "SneakerToSales",
                column: "SneakerProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_SneakerToSales_SneakerToSaleId",
                table: "Carts",
                column: "SneakerToSaleId",
                principalTable: "SneakerToSales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
