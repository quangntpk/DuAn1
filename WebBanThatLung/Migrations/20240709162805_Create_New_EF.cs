using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanThatLung.Migrations
{
    public partial class Create_New_EF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOAI_SAN_PHAMs",
                columns: table => new
                {
                    ID_LOAI_SAN_PHAM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TEN_LOAI_SAN_PHAM = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAI_SAN_PHAMs", x => x.ID_LOAI_SAN_PHAM);
                });

            migrationBuilder.CreateTable(
                name: "MAUs",
                columns: table => new
                {
                    ID_MAU = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAU = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAUs", x => x.ID_MAU);
                });

            migrationBuilder.CreateTable(
                name: "NGUOI_DUNGs",
                columns: table => new
                {
                    ID_NGUOI_DUNG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HO_TEN = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TAI_KHOAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MAT_KHAU = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VAI_TRO = table.Column<int>(type: "int", nullable: false),
                    HINH_ANH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NGAY_TAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TRANG_THAI = table.Column<int>(type: "int", nullable: false),
                    NGAY_SINH = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NGUOI_DUNGs", x => x.ID_NGUOI_DUNG);
                });

            migrationBuilder.CreateTable(
                name: "THUONG_HIEUs",
                columns: table => new
                {
                    ID_THUONG_HIEU = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TEN_THUONG_HIEU = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THUONG_HIEUs", x => x.ID_THUONG_HIEU);
                });

            migrationBuilder.CreateTable(
                name: "DON_HANGs",
                columns: table => new
                {
                    ID_DON_HANG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_NGUOI_DUNG = table.Column<int>(type: "int", nullable: false),
                    ID_NHAN_VIEN = table.Column<int>(type: "int", nullable: true),
                    NGAY_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TRANG_THAI_DH = table.Column<int>(type: "int", nullable: false),
                    TRANG_THAI_THANH_THAM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HinhThucThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NGAY_GIAO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LY_DO_HUY = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TEN_NGUOI_NHAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DIACHI = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DON_HANGs", x => x.ID_DON_HANG);
                    table.ForeignKey(
                        name: "FK_DON_HANGs_NGUOI_DUNGs_ID_NGUOI_DUNG",
                        column: x => x.ID_NGUOI_DUNG,
                        principalTable: "NGUOI_DUNGs",
                        principalColumn: "ID_NGUOI_DUNG",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DON_HANGs_NGUOI_DUNGs_ID_NHAN_VIEN",
                        column: x => x.ID_NHAN_VIEN,
                        principalTable: "NGUOI_DUNGs",
                        principalColumn: "ID_NGUOI_DUNG");
                });

            migrationBuilder.CreateTable(
                name: "SAN_PHAMs",
                columns: table => new
                {
                    ID_SAN_PHAM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TEN_SAN_PHAM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_LOAI_SAN_PHAM = table.Column<int>(type: "int", nullable: false),
                    ID_THUONG_HIEU = table.Column<int>(type: "int", nullable: false),
                    ID_MAU = table.Column<int>(type: "int", nullable: false),
                    SO_LUONG = table.Column<int>(type: "int", nullable: false),
                    GIA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CHAT_LIEU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MO_TA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NGAY_TAO = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAN_PHAMs", x => x.ID_SAN_PHAM);
                    table.ForeignKey(
                        name: "FK_SAN_PHAMs_LOAI_SAN_PHAMs_ID_LOAI_SAN_PHAM",
                        column: x => x.ID_LOAI_SAN_PHAM,
                        principalTable: "LOAI_SAN_PHAMs",
                        principalColumn: "ID_LOAI_SAN_PHAM",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SAN_PHAMs_MAUs_ID_MAU",
                        column: x => x.ID_MAU,
                        principalTable: "MAUs",
                        principalColumn: "ID_MAU",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SAN_PHAMs_THUONG_HIEUs_ID_THUONG_HIEU",
                        column: x => x.ID_THUONG_HIEU,
                        principalTable: "THUONG_HIEUs",
                        principalColumn: "ID_THUONG_HIEU",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_DON_HANGs",
                columns: table => new
                {
                    ID_CHI_TIET_DON_HANG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_DON_HANG = table.Column<int>(type: "int", nullable: false),
                    ID_SAN_PHAM = table.Column<int>(type: "int", nullable: false),
                    SO_LUONG = table.Column<int>(type: "int", nullable: false),
                    GIA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    THANH_TIEN = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHI_TIET_DON_HANGs", x => x.ID_CHI_TIET_DON_HANG);
                    table.ForeignKey(
                        name: "FK_CHI_TIET_DON_HANGs_DON_HANGs_ID_DON_HANG",
                        column: x => x.ID_DON_HANG,
                        principalTable: "DON_HANGs",
                        principalColumn: "ID_DON_HANG",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CHI_TIET_DON_HANGs_SAN_PHAMs_ID_SAN_PHAM",
                        column: x => x.ID_SAN_PHAM,
                        principalTable: "SAN_PHAMs",
                        principalColumn: "ID_SAN_PHAM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GIO_HANGs",
                columns: table => new
                {
                    ID_GIO_HANG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_NGUOI_DUNG = table.Column<int>(type: "int", nullable: false),
                    ID_SAN_PHAM = table.Column<int>(type: "int", nullable: false),
                    SO_LUONG_GH = table.Column<int>(type: "int", nullable: false),
                    THANH_TIEN = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GIA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MAU_SP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GIO_HANGs", x => x.ID_GIO_HANG);
                    table.ForeignKey(
                        name: "FK_GIO_HANGs_NGUOI_DUNGs_ID_NGUOI_DUNG",
                        column: x => x.ID_NGUOI_DUNG,
                        principalTable: "NGUOI_DUNGs",
                        principalColumn: "ID_NGUOI_DUNG",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GIO_HANGs_SAN_PHAMs_ID_SAN_PHAM",
                        column: x => x.ID_SAN_PHAM,
                        principalTable: "SAN_PHAMs",
                        principalColumn: "ID_SAN_PHAM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HINH_ANHs",
                columns: table => new
                {
                    ID_HINH_ANH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TEN_HINH_ANH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_SAN_PHAM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HINH_ANHs", x => x.ID_HINH_ANH);
                    table.ForeignKey(
                        name: "FK_HINH_ANHs_SAN_PHAMs_ID_SAN_PHAM",
                        column: x => x.ID_SAN_PHAM,
                        principalTable: "SAN_PHAMs",
                        principalColumn: "ID_SAN_PHAM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_DON_HANGs_ID_DON_HANG",
                table: "CHI_TIET_DON_HANGs",
                column: "ID_DON_HANG");

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_DON_HANGs_ID_SAN_PHAM",
                table: "CHI_TIET_DON_HANGs",
                column: "ID_SAN_PHAM");

            migrationBuilder.CreateIndex(
                name: "IX_DON_HANGs_ID_NGUOI_DUNG",
                table: "DON_HANGs",
                column: "ID_NGUOI_DUNG");

            migrationBuilder.CreateIndex(
                name: "IX_DON_HANGs_ID_NHAN_VIEN",
                table: "DON_HANGs",
                column: "ID_NHAN_VIEN");

            migrationBuilder.CreateIndex(
                name: "IX_GIO_HANGs_ID_NGUOI_DUNG",
                table: "GIO_HANGs",
                column: "ID_NGUOI_DUNG");

            migrationBuilder.CreateIndex(
                name: "IX_GIO_HANGs_ID_SAN_PHAM",
                table: "GIO_HANGs",
                column: "ID_SAN_PHAM");

            migrationBuilder.CreateIndex(
                name: "IX_HINH_ANHs_ID_SAN_PHAM",
                table: "HINH_ANHs",
                column: "ID_SAN_PHAM");

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAMs_ID_LOAI_SAN_PHAM",
                table: "SAN_PHAMs",
                column: "ID_LOAI_SAN_PHAM");

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAMs_ID_MAU",
                table: "SAN_PHAMs",
                column: "ID_MAU");

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAMs_ID_THUONG_HIEU",
                table: "SAN_PHAMs",
                column: "ID_THUONG_HIEU");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHI_TIET_DON_HANGs");

            migrationBuilder.DropTable(
                name: "GIO_HANGs");

            migrationBuilder.DropTable(
                name: "HINH_ANHs");

            migrationBuilder.DropTable(
                name: "DON_HANGs");

            migrationBuilder.DropTable(
                name: "SAN_PHAMs");

            migrationBuilder.DropTable(
                name: "NGUOI_DUNGs");

            migrationBuilder.DropTable(
                name: "LOAI_SAN_PHAMs");

            migrationBuilder.DropTable(
                name: "MAUs");

            migrationBuilder.DropTable(
                name: "THUONG_HIEUs");
        }
    }
}
