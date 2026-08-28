using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class first_table_IDMAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IDM_ACCOUNT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOMAIN_USER_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FULL_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDM_ACCOUNT", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IDM_ACCOUNT_DOMAIN_USER_NAME",
                table: "IDM_ACCOUNT",
                column: "DOMAIN_USER_NAME",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IDM_ACCOUNT");
        }
    }
}
