using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultipleFormat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newudpates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UPDATED_BY",
                table: "MFR_SUPERVISORS",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UPDATED_DATE",
                table: "MFR_SUPERVISORS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UPDATED_BY",
                table: "IDM_ACCOUNT",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UPDATED_DATE",
                table: "IDM_ACCOUNT",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supervisor_Active_DomainUserName",
                table: "MFR_SUPERVISORS",
                columns: new[] { "IS_ACTIVE", "DOMAIN_USER_NAME" });

            migrationBuilder.CreateIndex(
                name: "IX_IDMAccount_Active_DomainUserName",
                table: "IDM_ACCOUNT",
                columns: new[] { "IS_ACTIVE", "DOMAIN_USER_NAME" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Supervisor_Active_DomainUserName",
                table: "MFR_SUPERVISORS");

            migrationBuilder.DropIndex(
                name: "IX_IDMAccount_Active_DomainUserName",
                table: "IDM_ACCOUNT");

            migrationBuilder.DropColumn(
                name: "UPDATED_BY",
                table: "MFR_SUPERVISORS");

            migrationBuilder.DropColumn(
                name: "UPDATED_DATE",
                table: "MFR_SUPERVISORS");

            migrationBuilder.DropColumn(
                name: "UPDATED_BY",
                table: "IDM_ACCOUNT");

            migrationBuilder.DropColumn(
                name: "UPDATED_DATE",
                table: "IDM_ACCOUNT");
        }
    }
}
