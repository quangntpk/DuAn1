using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebBanThatLung.Models;
using WebBanThatLung.Repositoty;
using WebBanThatLung.ViewModels;

namespace WebBanThatLung.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaiAdminController : Controller
    {
        private readonly DataContext _dataContext;

        public LoaiAdminController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> TrangLoai()
        {
            var loaiSanPhams = await _dataContext.LOAI_SAN_PHAMs.ToListAsync();

            var viewModel = new LoaiSanPhamViewModel
            {
                LoaiSanPhams = loaiSanPhams,
                LoaiSanPham = new LoaiSanPhamModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemMoi(LoaiSanPhamModel loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Add(loaiSanPham);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Thêm loại sản phẩm thành công";
                return RedirectToAction(nameof(TrangLoai));
            }

            TempData["ThatBai"] = "Thêm loại sản phẩm thất bại";
            return View("TrangLoai", loaiSanPham);
        }

        public async Task<IActionResult> Edit(int id)
        {
            LoaiSanPhamModel loaiSanPham = await _dataContext.LOAI_SAN_PHAMs.FirstOrDefaultAsync(sp => sp.ID_LOAI_SAN_PHAM == id);

            return View(loaiSanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoaiSanPhamModel loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                var existingLoai = await _dataContext.LOAI_SAN_PHAMs.FirstOrDefaultAsync(sp => sp.ID_LOAI_SAN_PHAM == id);
                if (existingLoai == null)
                {
                    return NotFound();
                }

                existingLoai.TEN_LOAI_SAN_PHAM = loaiSanPham.TEN_LOAI_SAN_PHAM;

                _dataContext.Update(existingLoai);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Chỉnh sửa loại sản phẩm thành công";

                return RedirectToAction(nameof(TrangLoai));
            }

            TempData["ThatBai"] = "Sửa loại sản phẩm thất bại";
            return RedirectToAction(nameof(TrangLoai));
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var loaiSanPham = await _dataContext.LOAI_SAN_PHAMs.FindAsync(id);
            if (loaiSanPham == null)
            {
                TempData["ThatBai"] = "Không tìm thấy loại sản phẩm để xóa";
                return RedirectToAction(nameof(TrangLoai));
            }

            var sanPhams = await _dataContext.SAN_PHAMs.Where(sp => sp.ID_LOAI_SAN_PHAM == loaiSanPham.ID_LOAI_SAN_PHAM).ToListAsync();
            if (sanPhams.Any())
            {
                TempData["ThatBai"] = "Không thể xóa loại sản phẩm này vì còn sản phẩm thuộc loại này";
                return RedirectToAction(nameof(TrangLoai));
            }

            _dataContext.LOAI_SAN_PHAMs.Remove(loaiSanPham);
            await _dataContext.SaveChangesAsync();
            TempData["ThanhCong"] = "Xóa loại sản phẩm thành công";

            return RedirectToAction(nameof(TrangLoai));
        }

        public async Task<IActionResult> GetLoaiSanPham(int id)
        {
            var loaiSanPham = await _dataContext.LOAI_SAN_PHAMs
                .Where(sp => sp.ID_LOAI_SAN_PHAM == id)
                .Select(sp => new
                {
                    id = sp.ID_LOAI_SAN_PHAM,
                    tenLoaiSanPham = sp.TEN_LOAI_SAN_PHAM
                })
                .FirstOrDefaultAsync();

            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return Json(loaiSanPham);
        }





    }
}
