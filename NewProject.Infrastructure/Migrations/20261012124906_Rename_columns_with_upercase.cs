using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rename_columns_with_upercase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_MFR_FORM_REQUEST_MFR_EMPLOYEES_EmployeeId",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_MFR_PERMISSION_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                table: "MFR_PERMISSION_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_MFR_TIME_ADJUSTMENT_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                table: "MFR_TIME_ADJUSTMENT_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_MFR_VACATION_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                table: "MFR_VACATION_REQUEST");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MFR_VACATION_REQUEST",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "VacationDays",
                table: "MFR_VACATION_REQUEST",
                newName: "VACATION_DAYS");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "MFR_VACATION_REQUEST",
                newName: "START_DATE");

            migrationBuilder.RenameColumn(
                name: "ReturnDate",
                table: "MFR_VACATION_REQUEST",
                newName: "RETURN_DATE");

            migrationBuilder.RenameColumn(
                name: "FormRequestId",
                table: "MFR_VACATION_REQUEST",
                newName: "FORM_REQUEST_ID");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "MFR_VACATION_REQUEST",
                newName: "END_DATE");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_VACATION_REQUEST_FormRequestId",
                table: "MFR_VACATION_REQUEST",
                newName: "IX_MFR_VACATION_REQUEST_FORM_REQUEST_ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "MissingPunchType",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "MISSING_PUNCH_TYPE");

            migrationBuilder.RenameColumn(
                name: "FormRequestId",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "FORM_REQUEST_ID");

            migrationBuilder.RenameColumn(
                name: "ExitTime",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "EXIT_TIME");

            migrationBuilder.RenameColumn(
                name: "EntryTime",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "ENTRY_TIME");

            migrationBuilder.RenameColumn(
                name: "AdjustmentDate",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "ADJUSTMENT_DATE");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_TIME_ADJUSTMENT_REQUEST_FormRequestId",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "IX_MFR_TIME_ADJUSTMENT_REQUEST_FORM_REQUEST_ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MFR_PERMISSION_REQUEST",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "MFR_PERMISSION_REQUEST",
                newName: "START_DATE");

            migrationBuilder.RenameColumn(
                name: "PermissionDays",
                table: "MFR_PERMISSION_REQUEST",
                newName: "PERMISSION_DAYS");

            migrationBuilder.RenameColumn(
                name: "FormRequestId",
                table: "MFR_PERMISSION_REQUEST",
                newName: "FORM_REQUEST_ID");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "MFR_PERMISSION_REQUEST",
                newName: "END_DATE");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_PERMISSION_REQUEST_FormRequestId",
                table: "MFR_PERMISSION_REQUEST",
                newName: "IX_MFR_PERMISSION_REQUEST_FORM_REQUEST_ID");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "MFR_FORM_REQUEST",
                newName: "STATUS");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "MFR_FORM_REQUEST",
                newName: "COMMENTS");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MFR_FORM_REQUEST",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "SubmittedDate",
                table: "MFR_FORM_REQUEST",
                newName: "SUBMITTED_DATE");

            migrationBuilder.RenameColumn(
                name: "FormType",
                table: "MFR_FORM_REQUEST",
                newName: "FORM_TYPE");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "MFR_FORM_REQUEST",
                newName: "EMPLOYEE_ID");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "MFR_FORM_REQUEST",
                newName: "CREATED_DATE");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_FORM_REQUEST_EmployeeId",
                table: "MFR_FORM_REQUEST",
                newName: "IX_MFR_FORM_REQUEST_EMPLOYEE_ID");

            migrationBuilder.RenameColumn(
                name: "Shift",
                table: "MFR_EMPLOYEES",
                newName: "SHIFT");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "MFR_EMPLOYEES",
                newName: "EMAIL");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "MFR_EMPLOYEES",
                newName: "DEPARTMENT");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MFR_EMPLOYEES",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "MFR_EMPLOYEES",
                newName: "IS_ACTIVE");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "MFR_EMPLOYEES",
                newName: "FULL_NAME");

            migrationBuilder.RenameColumn(
                name: "EmployeeNumber",
                table: "MFR_EMPLOYEES",
                newName: "EMPLOYEE_NUMBER");

            migrationBuilder.RenameColumn(
                name: "DomainUserName",
                table: "MFR_EMPLOYEES",
                newName: "DOMAIN_USER_NAME");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_EMPLOYEES_EmployeeNumber",
                table: "MFR_EMPLOYEES",
                newName: "IX_MFR_EMPLOYEES_EMPLOYEE_NUMBER");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_EMPLOYEES_DomainUserName",
                table: "MFR_EMPLOYEES",
                newName: "IX_MFR_EMPLOYEES_DOMAIN_USER_NAME");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "REASON");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "NewSupervisor",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "NEW_SUPERVISOR");

            migrationBuilder.RenameColumn(
                name: "NewShift",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "NEW_SHIFT");

            migrationBuilder.RenameColumn(
                name: "NewPlant",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "NEW_PLANT");

            migrationBuilder.RenameColumn(
                name: "NewDepartment",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "NEW_DEPARTMENT");

            migrationBuilder.RenameColumn(
                name: "FormRequestId",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "FORM_REQUEST_ID");

            migrationBuilder.RenameColumn(
                name: "EffectiveDate",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "EFFECTIVE_DATE");

            migrationBuilder.RenameColumn(
                name: "CurrentSupervisor",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "CURRENT_SUPERVISOR");

            migrationBuilder.RenameColumn(
                name: "CurrentShift",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "CURRENT_SHIFT");

            migrationBuilder.RenameColumn(
                name: "CurrentPlant",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "CURRENT_PLANT");

            migrationBuilder.RenameColumn(
                name: "CurrentDepartment",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "CURRENT_DEPARTMENT");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST_FormRequestId",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "IX_MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST_FORM_REQUEST_ID");

            migrationBuilder.AddColumn<int>(
                name: "SUBMITTED_BY_ID",
                table: "MFR_FORM_REQUEST",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST_MFR_FORM_REQUEST_FORM_REQUEST_ID",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                column: "FORM_REQUEST_ID",
                principalTable: "MFR_FORM_REQUEST",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_FORM_REQUEST_MFR_EMPLOYEES_EMPLOYEE_ID",
                table: "MFR_FORM_REQUEST",
                column: "EMPLOYEE_ID",
                principalTable: "MFR_EMPLOYEES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_PERMISSION_REQUEST_MFR_FORM_REQUEST_FORM_REQUEST_ID",
                table: "MFR_PERMISSION_REQUEST",
                column: "FORM_REQUEST_ID",
                principalTable: "MFR_FORM_REQUEST",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_TIME_ADJUSTMENT_REQUEST_MFR_FORM_REQUEST_FORM_REQUEST_ID",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                column: "FORM_REQUEST_ID",
                principalTable: "MFR_FORM_REQUEST",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_VACATION_REQUEST_MFR_FORM_REQUEST_FORM_REQUEST_ID",
                table: "MFR_VACATION_REQUEST",
                column: "FORM_REQUEST_ID",
                principalTable: "MFR_FORM_REQUEST",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST_MFR_FORM_REQUEST_FORM_REQUEST_ID",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_MFR_FORM_REQUEST_MFR_EMPLOYEES_EMPLOYEE_ID",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_MFR_PERMISSION_REQUEST_MFR_FORM_REQUEST_FORM_REQUEST_ID",
                table: "MFR_PERMISSION_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_MFR_TIME_ADJUSTMENT_REQUEST_MFR_FORM_REQUEST_FORM_REQUEST_ID",
                table: "MFR_TIME_ADJUSTMENT_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_MFR_VACATION_REQUEST_MFR_FORM_REQUEST_FORM_REQUEST_ID",
                table: "MFR_VACATION_REQUEST");

            migrationBuilder.DropColumn(
                name: "SUBMITTED_BY_ID",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MFR_VACATION_REQUEST",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "VACATION_DAYS",
                table: "MFR_VACATION_REQUEST",
                newName: "VacationDays");

            migrationBuilder.RenameColumn(
                name: "START_DATE",
                table: "MFR_VACATION_REQUEST",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "RETURN_DATE",
                table: "MFR_VACATION_REQUEST",
                newName: "ReturnDate");

            migrationBuilder.RenameColumn(
                name: "FORM_REQUEST_ID",
                table: "MFR_VACATION_REQUEST",
                newName: "FormRequestId");

            migrationBuilder.RenameColumn(
                name: "END_DATE",
                table: "MFR_VACATION_REQUEST",
                newName: "EndDate");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_VACATION_REQUEST_FORM_REQUEST_ID",
                table: "MFR_VACATION_REQUEST",
                newName: "IX_MFR_VACATION_REQUEST_FormRequestId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MISSING_PUNCH_TYPE",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "MissingPunchType");

            migrationBuilder.RenameColumn(
                name: "FORM_REQUEST_ID",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "FormRequestId");

            migrationBuilder.RenameColumn(
                name: "EXIT_TIME",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "ExitTime");

            migrationBuilder.RenameColumn(
                name: "ENTRY_TIME",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "EntryTime");

            migrationBuilder.RenameColumn(
                name: "ADJUSTMENT_DATE",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "AdjustmentDate");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_TIME_ADJUSTMENT_REQUEST_FORM_REQUEST_ID",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                newName: "IX_MFR_TIME_ADJUSTMENT_REQUEST_FormRequestId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MFR_PERMISSION_REQUEST",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "START_DATE",
                table: "MFR_PERMISSION_REQUEST",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "PERMISSION_DAYS",
                table: "MFR_PERMISSION_REQUEST",
                newName: "PermissionDays");

            migrationBuilder.RenameColumn(
                name: "FORM_REQUEST_ID",
                table: "MFR_PERMISSION_REQUEST",
                newName: "FormRequestId");

            migrationBuilder.RenameColumn(
                name: "END_DATE",
                table: "MFR_PERMISSION_REQUEST",
                newName: "EndDate");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_PERMISSION_REQUEST_FORM_REQUEST_ID",
                table: "MFR_PERMISSION_REQUEST",
                newName: "IX_MFR_PERMISSION_REQUEST_FormRequestId");

            migrationBuilder.RenameColumn(
                name: "STATUS",
                table: "MFR_FORM_REQUEST",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "COMMENTS",
                table: "MFR_FORM_REQUEST",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MFR_FORM_REQUEST",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SUBMITTED_DATE",
                table: "MFR_FORM_REQUEST",
                newName: "SubmittedDate");

            migrationBuilder.RenameColumn(
                name: "FORM_TYPE",
                table: "MFR_FORM_REQUEST",
                newName: "FormType");

            migrationBuilder.RenameColumn(
                name: "EMPLOYEE_ID",
                table: "MFR_FORM_REQUEST",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "CREATED_DATE",
                table: "MFR_FORM_REQUEST",
                newName: "CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_FORM_REQUEST_EMPLOYEE_ID",
                table: "MFR_FORM_REQUEST",
                newName: "IX_MFR_FORM_REQUEST_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "SHIFT",
                table: "MFR_EMPLOYEES",
                newName: "Shift");

            migrationBuilder.RenameColumn(
                name: "EMAIL",
                table: "MFR_EMPLOYEES",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "DEPARTMENT",
                table: "MFR_EMPLOYEES",
                newName: "Department");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MFR_EMPLOYEES",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IS_ACTIVE",
                table: "MFR_EMPLOYEES",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "FULL_NAME",
                table: "MFR_EMPLOYEES",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "EMPLOYEE_NUMBER",
                table: "MFR_EMPLOYEES",
                newName: "EmployeeNumber");

            migrationBuilder.RenameColumn(
                name: "DOMAIN_USER_NAME",
                table: "MFR_EMPLOYEES",
                newName: "DomainUserName");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_EMPLOYEES_EMPLOYEE_NUMBER",
                table: "MFR_EMPLOYEES",
                newName: "IX_MFR_EMPLOYEES_EmployeeNumber");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_EMPLOYEES_DOMAIN_USER_NAME",
                table: "MFR_EMPLOYEES",
                newName: "IX_MFR_EMPLOYEES_DomainUserName");

            migrationBuilder.RenameColumn(
                name: "REASON",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NEW_SUPERVISOR",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "NewSupervisor");

            migrationBuilder.RenameColumn(
                name: "NEW_SHIFT",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "NewShift");

            migrationBuilder.RenameColumn(
                name: "NEW_PLANT",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "NewPlant");

            migrationBuilder.RenameColumn(
                name: "NEW_DEPARTMENT",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "NewDepartment");

            migrationBuilder.RenameColumn(
                name: "FORM_REQUEST_ID",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "FormRequestId");

            migrationBuilder.RenameColumn(
                name: "EFFECTIVE_DATE",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "EffectiveDate");

            migrationBuilder.RenameColumn(
                name: "CURRENT_SUPERVISOR",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "CurrentSupervisor");

            migrationBuilder.RenameColumn(
                name: "CURRENT_SHIFT",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "CurrentShift");

            migrationBuilder.RenameColumn(
                name: "CURRENT_PLANT",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "CurrentPlant");

            migrationBuilder.RenameColumn(
                name: "CURRENT_DEPARTMENT",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "CurrentDepartment");

            migrationBuilder.RenameIndex(
                name: "IX_MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST_FORM_REQUEST_ID",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                newName: "IX_MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST_FormRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                table: "MFR_DEPARTMENT_SHIFT_CHANGE_REQUEST",
                column: "FormRequestId",
                principalTable: "MFR_FORM_REQUEST",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_FORM_REQUEST_MFR_EMPLOYEES_EmployeeId",
                table: "MFR_FORM_REQUEST",
                column: "EmployeeId",
                principalTable: "MFR_EMPLOYEES",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_PERMISSION_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                table: "MFR_PERMISSION_REQUEST",
                column: "FormRequestId",
                principalTable: "MFR_FORM_REQUEST",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_TIME_ADJUSTMENT_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                table: "MFR_TIME_ADJUSTMENT_REQUEST",
                column: "FormRequestId",
                principalTable: "MFR_FORM_REQUEST",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_VACATION_REQUEST_MFR_FORM_REQUEST_FormRequestId",
                table: "MFR_VACATION_REQUEST",
                column: "FormRequestId",
                principalTable: "MFR_FORM_REQUEST",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
