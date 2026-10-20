using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultipleFormat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class new_relations_created : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "APPROVER_ID",
                table: "MFR_FORM_REQUEST",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MFR_FORM_REQUEST_APPROVER_ID",
                table: "MFR_FORM_REQUEST",
                column: "APPROVER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_APPROVER_ID",
                table: "MFR_FORM_REQUEST",
                column: "APPROVER_ID",
                principalTable: "IDM_ACCOUNT",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_APPROVER_ID",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.DropIndex(
                name: "IX_MFR_FORM_REQUEST_APPROVER_ID",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.DropColumn(
                name: "APPROVER_ID",
                table: "MFR_FORM_REQUEST");
        }
    }
}
