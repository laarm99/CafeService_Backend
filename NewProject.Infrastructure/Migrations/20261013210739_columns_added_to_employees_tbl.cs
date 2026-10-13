using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class columns_added_to_employees_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PLANT",
                table: "MFR_EMPLOYEES",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SUPERVISOR_NAME",
                table: "MFR_EMPLOYEES",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PLANT",
                table: "MFR_EMPLOYEES");

            migrationBuilder.DropColumn(
                name: "SUPERVISOR_NAME",
                table: "MFR_EMPLOYEES");
        }
    }
}
