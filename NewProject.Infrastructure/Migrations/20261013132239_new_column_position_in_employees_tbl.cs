using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class new_column_position_in_employees_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "POSITION",
                table: "MFR_EMPLOYEES",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "POSITION",
                table: "MFR_EMPLOYEES");
        }
    }
}
