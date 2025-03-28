using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTravel.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleAppServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleAppService",
                columns: table => new
                {
                    RoleAppServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppServiceId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAppService", x => x.RoleAppServiceId);
                    table.ForeignKey(
                        name: "FK_RoleAppService_AppServices_AppServiceId",
                        column: x => x.AppServiceId,
                        principalTable: "AppServices",
                        principalColumn: "AppServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleAppService_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAppService_AppServiceId",
                table: "RoleAppService",
                column: "AppServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAppService_RoleAppServiceId",
                table: "RoleAppService",
                column: "RoleAppServiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleAppService_RoleId",
                table: "RoleAppService",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleAppService");
        }
    }
}
