using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zdm_management.Migrations
{
    public partial class Leave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "leaves",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Leavedate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leaves", x => x.id);
                    table.ForeignKey(
                        name: "FK_leaves_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_leaves_ApplicationUserID",
                table: "leaves",
                column: "ApplicationUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "leaves");
        }
    }
}
