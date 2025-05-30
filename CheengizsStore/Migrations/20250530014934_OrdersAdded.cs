using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheengizsStore.Migrations
{
    /// <inheritdoc />
    public partial class OrdersAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Accounts_AccountId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_SneakerColors_SneakerColorId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_SneakerProducts_SneakerProductId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_Order_SneakerProductId",
                table: "Orders",
                newName: "IX_Orders_SneakerProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_SneakerColorId",
                table: "Orders",
                newName: "IX_Orders_SneakerColorId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CreatedAt",
                table: "Orders",
                newName: "IX_Orders_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Order_AccountId",
                table: "Orders",
                newName: "IX_Orders_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Accounts_AccountId",
                table: "Orders",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_SneakerColors_SneakerColorId",
                table: "Orders",
                column: "SneakerColorId",
                principalTable: "SneakerColors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_SneakerProducts_SneakerProductId",
                table: "Orders",
                column: "SneakerProductId",
                principalTable: "SneakerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Accounts_AccountId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_SneakerColors_SneakerColorId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_SneakerProducts_SneakerProductId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_SneakerProductId",
                table: "Order",
                newName: "IX_Order_SneakerProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_SneakerColorId",
                table: "Order",
                newName: "IX_Order_SneakerColorId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CreatedAt",
                table: "Order",
                newName: "IX_Order_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_AccountId",
                table: "Order",
                newName: "IX_Order_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Accounts_AccountId",
                table: "Order",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
