using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanThatLung.Migrations
{
    public partial class fdfd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CancelCount",
                table: "NGUOI_DUNGs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEndDate",
                table: "NGUOI_DUNGs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TRANG_THAI",
                table: "GIO_HANGs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelCount",
                table: "NGUOI_DUNGs");

            migrationBuilder.DropColumn(
                name: "LockoutEndDate",
                table: "NGUOI_DUNGs");

            migrationBuilder.DropColumn(
                name: "TRANG_THAI",
                table: "GIO_HANGs");
        }
    }
}
