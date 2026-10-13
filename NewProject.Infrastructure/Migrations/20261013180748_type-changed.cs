using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class typechanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PERMISSION_TYPE",
                table: "MFR_PERMISSION_REQUEST",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SWAP_DETAIL",
                table: "MFR_PERMISSION_REQUEST",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PERMISSION_TYPE",
                table: "MFR_PERMISSION_REQUEST");

            migrationBuilder.DropColumn(
                name: "SWAP_DETAIL",
                table: "MFR_PERMISSION_REQUEST");
        }
    }
}
