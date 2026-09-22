using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateMultiFormatTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MFR_FORM_REQUEST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FormType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MFR_FORM_REQUEST", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormRequestId = table.Column<int>(type: "int", nullable: false),
                    CurrentDepartment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewDepartment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentShift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewShift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                        column: x => x.FormRequestId,
                        principalTable: "MFR_FORM_REQUEST",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MFR_PERMISSION_REQUEST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormRequestId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PermissionDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MFR_PERMISSION_REQUEST", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MFR_PERMISSION_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                        column: x => x.FormRequestId,
                        principalTable: "MFR_FORM_REQUEST",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MFR_TIME_ADJUSTMENT_REQUEST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormRequestId = table.Column<int>(type: "int", nullable: false),
                    AdjustmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MissingPunchType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ExitTime = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MFR_TIME_ADJUSTMENT_REQUEST", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MFR_TIME_ADJUSTMENT_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                        column: x => x.FormRequestId,
                        principalTable: "MFR_FORM_REQUEST",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MFR_VACATION_REQUEST",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormRequestId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VacationDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MFR_VACATION_REQUEST", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MFR_VACATION_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                        column: x => x.FormRequestId,
                        principalTable: "MFR_FORM_REQUEST",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST_FormRequestId",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                column: "FormRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MFR_PERMISSION_REQUEST_FormRequestId",
                table: "MFR_PERMISSION_REQUEST",
                column: "FormRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MFR_TIME_ADJUSTMENT_REQUEST_FormRequestId",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                column: "FormRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MFR_VACATION_REQUEST_FormRequestId",
                table: "MFR_VACATION_REQUEST",
                column: "FormRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST");

            migrationBuilder.DropTable(
                name: "MFR_PERMISSION_REQUEST");

            migrationBuilder.DropTable(
                name: "MFR_TIME_ADJUSTMENT_REQUEST");

            migrationBuilder.DropTable(
                name: "MFR_VACATION_REQUEST");

            migrationBuilder.DropTable(
                name: "MFR_FORM_REQUEST");
        }
    }
}
