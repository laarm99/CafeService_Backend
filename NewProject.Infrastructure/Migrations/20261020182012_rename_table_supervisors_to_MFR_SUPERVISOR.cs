using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultipleFormat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rename_table_supervisors_to_MFR_SUPERVISOR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MFR_EMPLOYEES_SUPERVISORS_ID_SUPERVISOR",
                table: "MFR_EMPLOYEES");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SUPERVISORS",
                table: "SUPERVISORS");

            migrationBuilder.RenameTable(
                name: "SUPERVISORS",
                newName: "MFR_SUPERVISORS");

            migrationBuilder.RenameIndex(
                name: "IX_SUPERVISORS_NORMALIZED_NAME",
                table: "MFR_SUPERVISORS",
                newName: "IX_MFR_SUPERVISORS_NORMALIZED_NAME");

            migrationBuilder.RenameIndex(
                name: "IX_SUPERVISORS_DOMAIN_USER_NAME",
                table: "MFR_SUPERVISORS",
                newName: "IX_MFR_SUPERVISORS_DOMAIN_USER_NAME");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MFR_SUPERVISORS",
                table: "MFR_SUPERVISORS",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_EMPLOYEES_MFR_SUPERVISORS_ID_SUPERVISOR",
                table: "MFR_EMPLOYEES",
                column: "ID_SUPERVISOR",
                principalTable: "MFR_SUPERVISORS",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MFR_EMPLOYEES_MFR_SUPERVISORS_ID_SUPERVISOR",
                table: "MFR_EMPLOYEES");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MFR_SUPERVISORS",
                table: "MFR_SUPERVISORS");

            migrationBuilder.RenameTable(
                name: "MFR_SUPERVISORS",
                newName: "SUPERVISORS");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_SUPERVISORS_NORMALIZED_NAME",
                table: "SUPERVISORS",
                newName: "IX_SUPERVISORS_NORMALIZED_NAME");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_SUPERVISORS_DOMAIN_USER_NAME",
                table: "SUPERVISORS",
                newName: "IX_SUPERVISORS_DOMAIN_USER_NAME");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SUPERVISORS",
                table: "SUPERVISORS",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_EMPLOYEES_SUPERVISORS_ID_SUPERVISOR",
                table: "MFR_EMPLOYEES",
                column: "ID_SUPERVISOR",
                principalTable: "SUPERVISORS",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
