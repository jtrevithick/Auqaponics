using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auqaponics2.Data.Migrations
{
    public partial class specs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "System",
                columns: table => new
                {
                    SpecsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TankSize = table.Column<int>(nullable: false),
                    Plants = table.Column<string>(nullable: true),
                    Fish = table.Column<string>(nullable: true),
                    WaterTemp = table.Column<int>(nullable: false),
                    pH = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_System", x => x.SpecsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "System");
        }
    }
}
