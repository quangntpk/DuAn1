using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Repository;
using System.Linq;
using System.Threading.Tasks;
using WebBanThatLung.Repositoty;

namespace WebBanThatLung.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DonHangAdminController : Controller
    {
        private readonly DataContext _dataContext;

        public DonHangAdminController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> DonHang()
        {
            var donHang = await _dataContext.DON_HANGs
                                            .Include(dh => dh.NGUOI_DUNG)
                                            .Include(cc => cc.CHI_TIET_DON_HANG)
                                            .ThenInclude(sp => sp.SAN_PHAM)
                                            .ToListAsync();

            ViewBag.ChoDuyet = donHang.Where(d => d.TRANG_THAI_DH == 0).ToList();
            ViewBag.DangGiao = donHang.Where(d => d.TRANG_THAI_DH == 1).ToList();
            ViewBag.HoanThanh = donHang.Where(d => d.TRANG_THAI_DH == 2).ToList();
            return View(donHang);
        }

        [HttpPost]
        public async Task<IActionResult> DuyetDon(int id)
        {
            var donHang = await _dataContext.DON_HANGs.FindAsync(id);
            if(donHang != null)
            {
                donHang.TRANG_THAI_DH = 1;
                _dataContext.DON_HANGs.Update(donHang);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Duyệt đơn hàng thành công";

              
            }

            return RedirectToAction("DonHang");

        }

        [HttpPost]
        public async Task<IActionResult> HoanThanh(int id)
        {
            var donHang = await _dataContext.DON_HANGs.FindAsync(id);
            if (donHang != null)
            {
                donHang.TRANG_THAI_DH = 2;
                donHang.TRANG_THAI_THANH_THAM = "Đã thanh toán";
                donHang.NGAY_GIAO = DateTime.Now;
                _dataContext.DON_HANGs.Update(donHang);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Duyệt đơn hàng thành công";


            }

            return RedirectToAction("DonHang");

        }

        [HttpPost]
        public async Task<IActionResult> HuyDon(int id, string LyDoHuy)
        {
            var donHang = await _dataContext.DON_HANGs.FindAsync(id);
            if (donHang != null)
            {
                donHang.TRANG_THAI_THANH_THAM = "Chưa thanh toán";
                donHang.LY_DO_HUY = LyDoHuy;
                donHang.TRANG_THAI_DH = 5;
                _dataContext.Update(donHang);
                await _dataContext.SaveChangesAsync();
            }
            return RedirectToAction("DonHang");
        }

        public async Task<IActionResult> ChiTiet(int id)
        {
            var donHang = await _dataContext.DON_HANGs
                                     .Include(dh => dh.CHI_TIET_DON_HANG)
                                         .ThenInclude(ct => ct.SAN_PHAM)
                                     .Include(dh => dh.NGUOI_DUNG)
                                     .FirstOrDefaultAsync(dh => dh.ID_NGUOI_DUNG == id);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

    }
}
