using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;
using WebBanThatLung.Repository;
using WebBanThatLung.Repositoty;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanThatLung.Controllers
{
    public class GioHangController : Controller
    {
        private readonly DataContext _dataContext;

        public GioHangController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> TrangGioHang()
        {
            var khachHang = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (khachHang == null)
            {
                return RedirectToAction("Login", "NguoiDung");
            }

            var gioHang = await _dataContext.GIO_HANGs
                                            .Include(gh => gh.SAN_PHAM)
                                            .ThenInclude(sp => sp.MAU)
                                            .Include(gh => gh.SAN_PHAM)
                                            .ThenInclude(sp => sp.HINH_ANH)
                                            .Where(gh => gh.ID_NGUOI_DUNG == khachHang.ID_NGUOI_DUNG)
                                            .ToListAsync();

            decimal tongTien = gioHang.Sum(gh => gh.THANH_TIEN);

            ViewBag.TongTien = tongTien;
            ViewBag.tenKH = khachHang.HO_TEN;

            return View(gioHang);
        }


        [HttpPost]
        public async Task<IActionResult> ThemGioHang(int ID_SAN_PHAM, int SoLuong, int Mau)
        {
            var khachHang = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (khachHang == null)
            {
                return RedirectToAction("Login", "NguoiDung");
            }

            var sanPham = await _dataContext.SAN_PHAMs
                                            .Where(sp => sp.ID_SAN_PHAM == ID_SAN_PHAM && sp.ID_MAU == Mau)
                                            .FirstOrDefaultAsync();
            if (sanPham == null)
            {
                return NotFound("Sản phẩm hoặc màu sắc không tồn tại.");
            }

            var gioHangItem = await _dataContext.GIO_HANGs
                                                .Where(gh => gh.ID_SAN_PHAM == ID_SAN_PHAM && gh.MAU_SP == Mau && gh.ID_NGUOI_DUNG == khachHang.ID_NGUOI_DUNG)
                                                .FirstOrDefaultAsync();

            if (gioHangItem != null)
            {

                gioHangItem.SO_LUONG_GH += SoLuong;
                gioHangItem.THANH_TIEN += sanPham.GIA * SoLuong;
            }
            else
            {
    
                var gioHang = new GioHangModel
                {
                    ID_NGUOI_DUNG = khachHang.ID_NGUOI_DUNG,
                    ID_SAN_PHAM = sanPham.ID_SAN_PHAM,
                    SO_LUONG_GH = SoLuong,
                    GIA = sanPham.GIA,
                    THANH_TIEN = sanPham.GIA * SoLuong,
                    MAU_SP = Mau
                };

                _dataContext.GIO_HANGs.Add(gioHang);
            }

            sanPham.SO_LUONG -= SoLuong;
            await _dataContext.SaveChangesAsync();

            return RedirectToAction("TrangGioHang");
        }

        [HttpPost]
        public async Task<IActionResult> XoaKhoiGioHang(int maGH)
        {
            var khachHang = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (khachHang == null)
            {
                return Unauthorized();
            }

            var gioHang = await _dataContext.GIO_HANGs.FirstOrDefaultAsync(gh => gh.ID_GIO_HANG == maGH && gh.ID_NGUOI_DUNG == khachHang.ID_NGUOI_DUNG);
            if (gioHang == null)
            {
                return NotFound();
            }

            var sanPham = await _dataContext.SAN_PHAMs.FindAsync(gioHang.ID_SAN_PHAM);
            if (sanPham == null)
            {
                return NotFound();
            }
            sanPham.SO_LUONG += gioHang.SO_LUONG_GH;

            _dataContext.Update(sanPham);


            _dataContext.GIO_HANGs.Remove(gioHang);
            await _dataContext.SaveChangesAsync();

            TempData["ThanhCong"] = "Xóa khỏi giỏ hàng thành công";
            return RedirectToAction("TrangGioHang");
        }


        [HttpGet]
        public async Task<IActionResult> LaySoLuongSanPham(int idSanPham, int idMau)
        {
            var sanPham = await _dataContext.SAN_PHAMs
                                            .Where(sp => sp.ID_SAN_PHAM == idSanPham && sp.ID_MAU == idMau)
                                            .FirstOrDefaultAsync();

            if (sanPham == null)
            {
                return Json(new { success = false, soLuong = 0 });
            }

            return Json(new { success = true, soLuong = sanPham.SO_LUONG });
        }
    }
}
