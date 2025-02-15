using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTravel.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TestEntity_Id",
                table: "TestEntity",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TestEntity_Id",
                table: "TestEntity");
        }
    }
}
