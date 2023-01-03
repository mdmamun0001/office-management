using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zdm_management.Migrations
{
    public partial class AddColumnTypeToLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "leaves",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "leaves");
        }
    }
}
