using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IDM_Tables_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IDM_ROLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROLE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDM_ROLE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IDM_ACCOUNT_ROLE",
                columns: table => new
                {
                    ACCOUNT_ID = table.Column<int>(type: "int", nullable: false),
                    ROLE_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDM_ACCOUNT_ROLE", x => new { x.ACCOUNT_ID, x.ROLE_ID });
                    table.ForeignKey(
                        name: "FK_IDM_ACCOUNT_ROLE_IDM_ACCOUNT_ACCOUNT_ID",
                        column: x => x.ACCOUNT_ID,
                        principalTable: "IDM_ACCOUNT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IDM_ACCOUNT_ROLE_IDM_ROLE_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalTable: "IDM_ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IDM_ACCOUNT_ROLE_ROLE_ID",
                table: "IDM_ACCOUNT_ROLE",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_IDM_ROLE_ROLE_NAME",
                table: "IDM_ROLE",
                column: "ROLE_NAME",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IDM_ACCOUNT_ROLE");

            migrationBuilder.DropTable(
                name: "IDM_ROLE");
        }
    }
}
