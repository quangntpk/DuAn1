using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;

namespace WebBanThatLung.Repositoty
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<SanPhamModel> SAN_PHAMs { get; set; }
        public DbSet<HinhAnhModel> HINH_ANHs { get; set; }
        public DbSet<MauModel> MAUs { get; set; }
        public DbSet<ThuongHieuModel> THUONG_HIEUs { get; set; }
        public DbSet<LoaiSanPhamModel> LOAI_SAN_PHAMs { get; set; }
        public DbSet<DonHangModel> DON_HANGs { get; set; }
        public DbSet<ChiTietDonHangModel> CHI_TIET_DON_HANGs { get; set; }
        public DbSet<NguoiDungModel> NGUOI_DUNGs { get; set; }
        public DbSet<GioHangModel> GIO_HANGs { get; set; }
        public DbSet<SanPhamMauModel> SAN_PHAM_MAUs { get; set; }



    }
}
