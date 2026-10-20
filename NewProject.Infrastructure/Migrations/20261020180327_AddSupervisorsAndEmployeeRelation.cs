using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultipleFormat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSupervisorsAndEmployeeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID_SUPERVISOR",
                table: "MFR_EMPLOYEES",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SUPERVISORS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FULL_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NORMALIZED_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DOMAIN_USER_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EMAIL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPERVISORS", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MFR_EMPLOYEES_ID_SUPERVISOR",
                table: "MFR_EMPLOYEES",
                column: "ID_SUPERVISOR");

            migrationBuilder.CreateIndex(
                name: "IX_SUPERVISORS_DOMAIN_USER_NAME",
                table: "SUPERVISORS",
                column: "DOMAIN_USER_NAME",
                unique: true,
                filter: "[DOMAIN_USER_NAME] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SUPERVISORS_NORMALIZED_NAME",
                table: "SUPERVISORS",
                column: "NORMALIZED_NAME",
                unique: true);

            migrationBuilder.Sql("""
                INSERT INTO SUPERVISORS (FULL_NAME, NORMALIZED_NAME, IS_ACTIVE, CREATED_DATE)
                SELECT MIN(LTRIM(RTRIM(SUPERVISOR_NAME))),
                       UPPER(LTRIM(RTRIM(SUPERVISOR_NAME))),
                       1,
                       GETUTCDATE()
                FROM MFR_EMPLOYEES
                WHERE SUPERVISOR_NAME IS NOT NULL
                  AND LTRIM(RTRIM(SUPERVISOR_NAME)) <> ''
                GROUP BY UPPER(LTRIM(RTRIM(SUPERVISOR_NAME)));

                UPDATE employee
                SET ID_SUPERVISOR = supervisor.ID
                FROM MFR_EMPLOYEES employee
                INNER JOIN SUPERVISORS supervisor
                    ON supervisor.NORMALIZED_NAME = UPPER(LTRIM(RTRIM(employee.SUPERVISOR_NAME)))
                WHERE employee.SUPERVISOR_NAME IS NOT NULL
                  AND LTRIM(RTRIM(employee.SUPERVISOR_NAME)) <> '';
                """);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_EMPLOYEES_SUPERVISORS_ID_SUPERVISOR",
                table: "MFR_EMPLOYEES",
                column: "ID_SUPERVISOR",
                principalTable: "SUPERVISORS",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MFR_EMPLOYEES_SUPERVISORS_ID_SUPERVISOR",
                table: "MFR_EMPLOYEES");

            migrationBuilder.DropTable(
                name: "SUPERVISORS");

            migrationBuilder.DropIndex(
                name: "IX_MFR_EMPLOYEES_ID_SUPERVISOR",
                table: "MFR_EMPLOYEES");

            migrationBuilder.DropColumn(
                name: "ID_SUPERVISOR",
                table: "MFR_EMPLOYEES");
        }
    }
}
