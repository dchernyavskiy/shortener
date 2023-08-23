using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortener.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "urls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "urls");
        }
    }
}
