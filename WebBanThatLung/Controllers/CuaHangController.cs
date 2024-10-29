using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using WebBanThatLung.Repositoty;
using WebBanThatLung.Repository;

namespace WebBanThatLung.Controllers
{
    public class CuaHangController : Controller
    {
        private readonly DataContext _dataContext;

        public CuaHangController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Phương thức hiển thị trang chính của cửa hàng với phân trang
        public async Task<IActionResult> Index(int page = 1, int pageSize = 12)
        {
            var sanPhamQuery = _dataContext.SAN_PHAMs
                                    .Include(sp => sp.HINH_ANH)
                                    .Where(sp => sp.SO_LUONG > 0)
                                    .AsQueryable();

            var sanPham = await sanPhamQuery
                            .ToPagedListAsync(page, pageSize);

            return View(sanPham);
        }

        // Phương thức tìm kiếm sản phẩm theo từ khóa và khoảng giá
        public async Task<IActionResult> Search(string searchQuery, int minPrice = 0, int maxPrice = 1000000)
        {
            var sanPhamQuery = _dataContext.SAN_PHAMs
                                .Include(sp => sp.HINH_ANH)
                                .Where(sp => sp.SO_LUONG > 0) // Lọc sản phẩm có số lượng > 0
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                sanPhamQuery = sanPhamQuery.Where(sp => sp.TEN_SAN_PHAM.Contains(searchQuery));
            }

            sanPhamQuery = sanPhamQuery.Where(sp => sp.GIA >= minPrice && sp.GIA <= maxPrice);

            var sanPham = await sanPhamQuery.ToListAsync();
            return PartialView("_ProductList", sanPham);
        }

        // Phương thức hiển thị chi tiết sản phẩm
        public async Task<IActionResult> ChiTietSP(int id)
        {
            var khachHang = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (khachHang == null)
            {
                return RedirectToAction("Login", "NguoiDung");
            }

            var sanPham = await _dataContext.SAN_PHAMs
                                            .Include(sp => sp.HINH_ANH)
                                            .Include(sp => sp.THUONG_HIEU)
                                            .Include(sp => sp.LOAI_SAN_PHAM)
                                            .FirstOrDefaultAsync(sp => sp.ID_SAN_PHAM == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            var danhSachMau = await _dataContext.MAUs
                                                .Where(mau => _dataContext.SAN_PHAM_MAUs
                                                                           .Any(spm => spm.ID_MAU == mau.ID_MAU))
                                                .ToListAsync();
            ViewBag.DanhSachMau = danhSachMau;

            var sanPhamLienQuan = await _dataContext.SAN_PHAMs
                .Include(sp => sp.HINH_ANH)
                .Where(sp => sp.ID_LOAI_SAN_PHAM == sanPham.ID_LOAI_SAN_PHAM && sp.ID_SAN_PHAM != id)
                .Take(4)
                .ToListAsync();

            ViewBag.HinhAnhLienQuan = sanPhamLienQuan.Select(sp => sp.HINH_ANH.FirstOrDefault()).ToList();
            ViewBag.SanPhamLienQuan = sanPhamLienQuan;

            return View(sanPham);
        }
    }
}
