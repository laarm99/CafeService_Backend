using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NewProject.Infrastructure.Persistence;

#nullable disable

namespace MultipleFormat.Infrastructure.Migrations
{
    // Los enums se guardan como texto: se renombra Booyh -> Both y se verifica que no queden
    // restricciones CHECK sobre STATUS que impidan el valor 'Completed'.
    [DbContext(typeof(MultipleFormatDbContext))]
    [Migration("20261023180000_NormalizeEnumValues")]
    public partial class NormalizeEnumValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE MFR_TIME_ADJUSTMENT_REQUEST SET MISSING_PUNCH_TYPE = 'Both' WHERE MISSING_PUNCH_TYPE = 'Booyh';");

            migrationBuilder.Sql(@"
DECLARE @sql nvarchar(max) = N'';
SELECT @sql += N'ALTER TABLE MFR_FORM_REQUEST DROP CONSTRAINT ' + QUOTENAME(cc.name) + N';'
FROM sys.check_constraints cc
WHERE cc.parent_object_id = OBJECT_ID(N'MFR_FORM_REQUEST')
  AND cc.parent_column_id = COLUMNPROPERTY(OBJECT_ID(N'MFR_FORM_REQUEST'), 'STATUS', 'ColumnId');
EXEC sp_executesql @sql;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE MFR_TIME_ADJUSTMENT_REQUEST SET MISSING_PUNCH_TYPE = 'Booyh' WHERE MISSING_PUNCH_TYPE = 'Both';");
        }
    }
}
