using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanThatLung.Migrations
{
    public partial class TaoLaiSql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SAN_PHAMs_MAUs_ID_MAU",
                table: "SAN_PHAMs");

            migrationBuilder.DropIndex(
                name: "IX_SAN_PHAMs_ID_MAU",
                table: "SAN_PHAMs");

            migrationBuilder.DropColumn(
                name: "ID_MAU",
                table: "SAN_PHAMs");

            migrationBuilder.CreateTable(
                name: "SAN_PHAM_MAUs",
                columns: table => new
                {
                    ID_MauSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_SAN_PHAM = table.Column<int>(type: "int", nullable: false),
                    ID_MAU = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAN_PHAM_MAUs", x => x.ID_MauSanPham);
                    table.ForeignKey(
                        name: "FK_SAN_PHAM_MAUs_MAUs_ID_MAU",
                        column: x => x.ID_MAU,
                        principalTable: "MAUs",
                        principalColumn: "ID_MAU",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SAN_PHAM_MAUs_SAN_PHAMs_ID_SAN_PHAM",
                        column: x => x.ID_SAN_PHAM,
                        principalTable: "SAN_PHAMs",
                        principalColumn: "ID_SAN_PHAM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAM_MAUs_ID_MAU",
                table: "SAN_PHAM_MAUs",
                column: "ID_MAU");

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAM_MAUs_ID_SAN_PHAM",
                table: "SAN_PHAM_MAUs",
                column: "ID_SAN_PHAM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAN_PHAM_MAUs");

            migrationBuilder.AddColumn<int>(
                name: "ID_MAU",
                table: "SAN_PHAMs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAMs_ID_MAU",
                table: "SAN_PHAMs",
                column: "ID_MAU");

            migrationBuilder.AddForeignKey(
                name: "FK_SAN_PHAMs_MAUs_ID_MAU",
                table: "SAN_PHAMs",
                column: "ID_MAU",
                principalTable: "MAUs",
                principalColumn: "ID_MAU",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
