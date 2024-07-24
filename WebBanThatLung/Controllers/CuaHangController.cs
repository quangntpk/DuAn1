using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;
using System.Linq;
using System.Threading.Tasks;
using WebBanThatLung.Repositoty;

namespace WebBanThatLung.Controllers
{
    public class CuaHangController : Controller
    {
        private readonly DataContext _dataContext;

        public CuaHangController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Phương thức hiển thị trang chính của cửa hàng
        public async Task<IActionResult> Index()
        {
            var sanPham = await _dataContext.SAN_PHAMs
                             .Include(sp => sp.HINH_ANH)
                             .Where(sp => sp.SO_LUONG > 0) // Lọc sản phẩm có số lượng > 0
                             .Take(8)
                             .ToListAsync();
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
            var sanPham = await _dataContext.SAN_PHAMs
                                            .Include(sp => sp.HINH_ANH)
                                            .Include(sp => sp.THUONG_HIEU)
                                            .Include(sp => sp.LOAI_SAN_PHAM)
                                            .Include(sp => sp.MAU)
                                            .FirstOrDefaultAsync(sp => sp.ID_SAN_PHAM == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            var danhSachMau = await _dataContext.MAUs.ToListAsync();
            ViewBag.DanhSachMau = danhSachMau;

            return View(sanPham);
        }
    }
}
