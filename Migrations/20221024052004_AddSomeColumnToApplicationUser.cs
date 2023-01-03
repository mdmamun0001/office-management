using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zdm_management.Migrations
{
    public partial class AddSomeColumnToApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "AspNetUsers");
        }
    }
}
