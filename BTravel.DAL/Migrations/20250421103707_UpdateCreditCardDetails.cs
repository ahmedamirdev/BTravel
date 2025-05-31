using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTravel.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreditCardDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "CommonUsers");

            migrationBuilder.DropColumn(
                name: "CardCVC",
                table: "CommonUsers");

            migrationBuilder.DropColumn(
                name: "CardExpDate",
                table: "CommonUsers");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "CommonUsers");

            migrationBuilder.DropColumn(
                name: "NameOnCreditCard",
                table: "CommonUsers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "CommonUsers");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "Contracts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CardCVC",
                table: "Contracts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CardExpDate",
                table: "Contracts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Contracts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NameOnCreditCard",
                table: "Contracts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Contracts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "ContractRooms",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "ContractRooms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CardCVC",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CardExpDate",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "NameOnCreditCard",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "ContractRooms");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "ContractRooms");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "CommonUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CardCVC",
                table: "CommonUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CardExpDate",
                table: "CommonUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "CommonUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NameOnCreditCard",
                table: "CommonUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "CommonUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
