using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultipleFormat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class new_relations_created_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_APPROVER_ID",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_CREATED_BY_ID",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_APPROVER_ID",
                table: "MFR_FORM_REQUEST",
                column: "APPROVER_ID",
                principalTable: "IDM_ACCOUNT",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_CREATED_BY_ID",
                table: "MFR_FORM_REQUEST",
                column: "CREATED_BY_ID",
                principalTable: "IDM_ACCOUNT",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_APPROVER_ID",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.DropForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_CREATED_BY_ID",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_APPROVER_ID",
                table: "MFR_FORM_REQUEST",
                column: "APPROVER_ID",
                principalTable: "IDM_ACCOUNT",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_CREATED_BY_ID",
                table: "MFR_FORM_REQUEST",
                column: "CREATED_BY_ID",
                principalTable: "IDM_ACCOUNT",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
