using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class New_table_employee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "MFR_FORM_REQUEST",
                type: "int",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "CurrentPlant",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentSupervisor",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewPlant",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewSupervisor",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MFR_EMPLOYEES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DomainUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MFR_EMPLOYEES", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MFR_FORM_REQUEST_EmployeeId",
                table: "MFR_FORM_REQUEST",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MFR_EMPLOYEES_DomainUserName",
                table: "MFR_EMPLOYEES",
                column: "DomainUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MFR_EMPLOYEES_EmployeeNumber",
                table: "MFR_EMPLOYEES",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_FORM_REQUEST_MFR_EMPLOYEES_EmployeeId",
                table: "MFR_FORM_REQUEST",
                column: "EmployeeId",
                principalTable: "MFR_EMPLOYEES",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MFR_FORM_REQUEST_MFR_EMPLOYEES_EmployeeId",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.DropTable(
                name: "MFR_EMPLOYEES");

            migrationBuilder.DropIndex(
                name: "IX_MFR_FORM_REQUEST_EmployeeId",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.DropColumn(
                name: "CurrentPlant",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST");

            migrationBuilder.DropColumn(
                name: "CurrentSupervisor",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST");

            migrationBuilder.DropColumn(
                name: "NewPlant",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST");

            migrationBuilder.DropColumn(
                name: "NewSupervisor",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "MFR_FORM_REQUEST",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50);
        }
    }
}
