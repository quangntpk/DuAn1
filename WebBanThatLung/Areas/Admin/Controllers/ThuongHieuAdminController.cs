using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;
using WebBanThatLung.Repositoty;
using WebBanThatLung.ViewModels;

namespace WebBanThatLung.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThuongHieuAdminController : Controller
    {
        private readonly DataContext _dataContext;

        public ThuongHieuAdminController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> TrangThuongHieu()
        {
            var XuatThuongHieu = await _dataContext.THUONG_HIEUs.ToListAsync();

            var viewModel = new ThuongHieuViewModel()
            {
                DanhSachThuongHieu = XuatThuongHieu,
                ThuongHieu = new Models.ThuongHieuModel()
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemMoi(ThuongHieuModel Thuonghieu)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Add(Thuonghieu);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Thêm thương hiệu thành công";
                return RedirectToAction(nameof(TrangThuongHieu));
            }

            TempData["ThatBai"] = "Thêm thương hiệu thất bại";
            return View("TrangThuongHieu", Thuonghieu);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ThuongHieuModel ThuongHieu = await _dataContext.THUONG_HIEUs.FirstOrDefaultAsync(sp => sp.ID_THUONG_HIEU == id);

            return View(ThuongHieu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, ThuongHieuModel ThuongHieu)
        {
            if (ModelState.IsValid)
            {
                var existingThuongHieu = await _dataContext.THUONG_HIEUs.FirstOrDefaultAsync(sp => sp.ID_THUONG_HIEU == id);
                if (existingThuongHieu == null)
                {
                    return NotFound();
                }

                existingThuongHieu.TEN_THUONG_HIEU = ThuongHieu.TEN_THUONG_HIEU;

                _dataContext.Update(existingThuongHieu);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Sửa thương hiệu thành công";

                return RedirectToAction(nameof(TrangThuongHieu));
            }

            TempData["ThatBai"] = "Sửa thương hiệu thất bại";
            return RedirectToAction(nameof(TrangThuongHieu));
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var Thuonghieu = await _dataContext.THUONG_HIEUs.FindAsync(id);
            if (Thuonghieu == null)
            {
                TempData["ThatBai"] = "Không tìm thấy thương hiệu để xóa";
                return RedirectToAction(nameof(TrangThuongHieu));
            }

            var sanPhams = await _dataContext.SAN_PHAMs.Where(sp => sp.ID_THUONG_HIEU == Thuonghieu.ID_THUONG_HIEU).ToListAsync();
            if (sanPhams.Any())
            {
                TempData["ThatBai"] = "Không thể xóa thương hiệu này vì còn sản phẩm thuộc loại này";
                return RedirectToAction(nameof(TrangThuongHieu));
            }

            _dataContext.THUONG_HIEUs.Remove(Thuonghieu);
            await _dataContext.SaveChangesAsync();
            TempData["ThanhCong"] = "Xóa thương hiệu thành công";

            return RedirectToAction(nameof(TrangThuongHieu));
        }




    }
}
