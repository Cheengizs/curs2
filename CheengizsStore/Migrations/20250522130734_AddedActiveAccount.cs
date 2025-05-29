using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheengizsStore.Migrations
{
    /// <inheritdoc />
    public partial class AddedActiveAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Sneakers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Sneakers");
        }
    }
}
