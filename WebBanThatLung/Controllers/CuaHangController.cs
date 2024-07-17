using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;
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

        public IActionResult Index()
        {
            return View();
        }

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
