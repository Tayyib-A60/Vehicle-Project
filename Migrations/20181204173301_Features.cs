using Microsoft.EntityFrameworkCore.Migrations;

namespace Vega.Migrations
{
    public partial class Features : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Feature (FeatureName) VALUES ('Feature5')");
            migrationBuilder.Sql("INSERT INTO Feature (FeatureName) VALUES ('Feature6')");
            migrationBuilder.Sql("INSERT INTO Feature (FeatureName) VALUES ('Feature7')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Feature WHERE FeatureName IN ('Feature5', 'Feature6', 'Feature7')");
        }
    }
}
