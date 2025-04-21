using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTravel.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContractFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileURL",
                table: "ContractFiles",
                newName: "FileUrl");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ContractFiles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ContractFiles");

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "ContractFiles",
                newName: "FileURL");
        }
    }
}
