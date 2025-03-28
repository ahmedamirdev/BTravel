using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTravel.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleAppServiceTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleAppService_AppServices_AppServiceId",
                table: "RoleAppService");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleAppService_Roles_RoleId",
                table: "RoleAppService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleAppService",
                table: "RoleAppService");

            migrationBuilder.RenameTable(
                name: "RoleAppService",
                newName: "RoleAppServices");

            migrationBuilder.RenameIndex(
                name: "IX_RoleAppService_RoleId",
                table: "RoleAppServices",
                newName: "IX_RoleAppServices_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleAppService_RoleAppServiceId",
                table: "RoleAppServices",
                newName: "IX_RoleAppServices_RoleAppServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleAppService_AppServiceId",
                table: "RoleAppServices",
                newName: "IX_RoleAppServices_AppServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleAppServices",
                table: "RoleAppServices",
                column: "RoleAppServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleAppServices_AppServices_AppServiceId",
                table: "RoleAppServices",
                column: "AppServiceId",
                principalTable: "AppServices",
                principalColumn: "AppServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleAppServices_Roles_RoleId",
                table: "RoleAppServices",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleAppServices_AppServices_AppServiceId",
                table: "RoleAppServices");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleAppServices_Roles_RoleId",
                table: "RoleAppServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleAppServices",
                table: "RoleAppServices");

            migrationBuilder.RenameTable(
                name: "RoleAppServices",
                newName: "RoleAppService");

            migrationBuilder.RenameIndex(
                name: "IX_RoleAppServices_RoleId",
                table: "RoleAppService",
                newName: "IX_RoleAppService_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleAppServices_RoleAppServiceId",
                table: "RoleAppService",
                newName: "IX_RoleAppService_RoleAppServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleAppServices_AppServiceId",
                table: "RoleAppService",
                newName: "IX_RoleAppService_AppServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleAppService",
                table: "RoleAppService",
                column: "RoleAppServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleAppService_AppServices_AppServiceId",
                table: "RoleAppService",
                column: "AppServiceId",
                principalTable: "AppServices",
                principalColumn: "AppServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleAppService_Roles_RoleId",
                table: "RoleAppService",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
