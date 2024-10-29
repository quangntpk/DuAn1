using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanThatLung.Migrations
{
    public partial class ThemCot_ChiTietDonHang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MauSanPham",
                table: "CHI_TIET_DON_HANGs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MauSanPham",
                table: "CHI_TIET_DON_HANGs");
        }
    }
}
