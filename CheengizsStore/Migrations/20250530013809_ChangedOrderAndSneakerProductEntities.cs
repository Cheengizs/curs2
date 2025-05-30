using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheengizsStore.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOrderAndSneakerProductEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_SneakerColors_SneakerColorId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "SneakerColorId",
                table: "Order",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SneakerProductId",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_SneakerProductId",
                table: "Order",
                column: "SneakerProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_SneakerColors_SneakerColorId",
                table: "Order",
                column: "SneakerColorId",
                principalTable: "SneakerColors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_SneakerProducts_SneakerProductId",
                table: "Order",
                column: "SneakerProductId",
                principalTable: "SneakerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_SneakerColors_SneakerColorId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_SneakerProducts_SneakerProductId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_SneakerProductId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "SneakerProductId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "SneakerColorId",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_SneakerColors_SneakerColorId",
                table: "Order",
                column: "SneakerColorId",
                principalTable: "SneakerColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
