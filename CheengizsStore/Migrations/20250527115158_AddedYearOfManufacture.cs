using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheengizsStore.Migrations
{
    /// <inheritdoc />
    public partial class AddedYearOfManufacture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearOfManufacture",
                table: "Sneakers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearOfManufacture",
                table: "Sneakers");
        }
    }
}
