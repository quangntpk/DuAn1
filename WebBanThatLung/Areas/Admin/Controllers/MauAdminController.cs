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

    public class MauAdminController : Controller
    {
        private readonly DataContext _dataContext;

        public MauAdminController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> TrangMau()
        {
            var Maus = await _dataContext.MAUs.ToListAsync();

            var viewModel = new MauViewModel()
            {
                DANHSACHMAU = Maus,
                mau = new MauModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemMau(MauModel MAU)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Add(MAU);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Thêm màu thành công";
                return RedirectToAction(nameof(TrangMau));
            }

            TempData["ThatBai"] = "Thêm màu thất bại";
            return View("TrangLoai", MAU);
        }

        public async Task<IActionResult> Edit(int id)
        {
            MauModel mau = await _dataContext.MAUs.FirstOrDefaultAsync(sp => sp.ID_MAU == id);

            return View(mau);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, MauModel ThuongHieu)
        {
            if (ModelState.IsValid)
            {
                var existingThuongHieu = await _dataContext.MAUs.FirstOrDefaultAsync(sp => sp.ID_MAU == id);
                if (existingThuongHieu == null)
                {
                    return NotFound();
                }

                existingThuongHieu.MAU = ThuongHieu.MAU;

                _dataContext.Update(existingThuongHieu);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Sửa màu thành công";

                return RedirectToAction(nameof(TrangMau));
            }

            TempData["ThatBai"] = "Sửa màu thất bại";
            return RedirectToAction(nameof(TrangMau));
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var MAU = await _dataContext.MAUs.FindAsync(id);
            if (MAU == null)
            {
                TempData["ThatBai"] = "Không tìm thấy màu để xóa";
                return RedirectToAction(nameof(TrangMau));
            }

            var sanPhams = await _dataContext.SAN_PHAMs.Where(sp => sp.ID_MAU == MAU.ID_MAU).ToListAsync();
            if (sanPhams.Any())
            {
                TempData["ThatBai"] = "Không thể xóa màu này vì còn sản phẩm đang con màu này";
                return RedirectToAction(nameof(TrangMau));
            }

            _dataContext.MAUs.Remove(MAU);
            await _dataContext.SaveChangesAsync();
            TempData["ThanhCong"] = "Xóa màu thành công";

            return RedirectToAction(nameof(TrangMau));
        }
    }
}
