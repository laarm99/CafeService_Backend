using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultipleFormat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class news_column_and_relation_created : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CREATED_BY_ID",
                table: "MFR_FORM_REQUEST",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MFR_FORM_REQUEST_CREATED_BY_ID",
                table: "MFR_FORM_REQUEST",
                column: "CREATED_BY_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_CREATED_BY_ID",
                table: "MFR_FORM_REQUEST",
                column: "CREATED_BY_ID",
                principalTable: "IDM_ACCOUNT",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MFR_FORM_REQUEST_IDM_ACCOUNT_CREATED_BY_ID",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.DropIndex(
                name: "IX_MFR_FORM_REQUEST_CREATED_BY_ID",
                table: "MFR_FORM_REQUEST");

            migrationBuilder.DropColumn(
                name: "CREATED_BY_ID",
                table: "MFR_FORM_REQUEST");
        }
    }
}
