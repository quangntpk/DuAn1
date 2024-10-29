using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebBanThatLung.Models;
using WebBanThatLung.Repositoty;
using X.PagedList;

namespace WebBanThatLung.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhanVienAdminController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NhanVienAdminController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [Route("Admin/NhanVienAdmin")]
        public async Task<IActionResult> NhanVienAdmin(int page = 1, int pageSize = 10)
        {
            var nguoiDungs = await _dataContext.NGUOI_DUNGs
                .Where(n => n.VAI_TRO == 2)
                .ToListAsync();

            var PhanTrangNhanVien = nguoiDungs.ToPagedList(page, pageSize);

            return View(PhanTrangNhanVien);
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NguoiDungModel nguoiDungModel, IFormFile HINH_ANH)
        {
            if (ModelState.IsValid)
            {
                var kiemtraTaiKhoan = await _dataContext.NGUOI_DUNGs.FirstOrDefaultAsync(m => m.TAI_KHOAN == nguoiDungModel.TAI_KHOAN);
                var kiemtraCCCD = await _dataContext.NGUOI_DUNGs.FirstOrDefaultAsync(m => m.CCCD == nguoiDungModel.CCCD);
                var kiemtraSDT = await _dataContext.NGUOI_DUNGs.FirstOrDefaultAsync(m => m.SDT == nguoiDungModel.SDT);
                var kiemtraEmail = await _dataContext.NGUOI_DUNGs.FirstOrDefaultAsync(m => m.EMAIL == nguoiDungModel.EMAIL);

                if (kiemtraTaiKhoan != null)
                {
                    TempData["ThatBai"] = "Tài khoản đã có người sử dụng.";
                    return View(nguoiDungModel);
                }
                else if (kiemtraCCCD != null)
                {
                    TempData["ThatBai"] = "CCCD đã có người sử dụng.";
                    return View(nguoiDungModel);
                }
                else if (kiemtraSDT != null)
                {
                    TempData["ThatBai"] = "Số điện thoại đã có người sử dụng.";
                    return View(nguoiDungModel);
                }
                else if (kiemtraEmail != null)
                {
                    TempData["ThatBai"] = "Email đã có người sử dụng.";
                    return View(nguoiDungModel);
                }

                // Handle file upload
                if (HINH_ANH != null && HINH_ANH.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(HINH_ANH.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("HINH_ANH", "Chỉ chấp nhận các định dạng ảnh: .jpg, .jpeg, .png");
                        return View(nguoiDungModel);
                    }

                    var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/NhanVien1");
                    var imageName = Guid.NewGuid().ToString() + "_" + HINH_ANH.FileName;
                    var filePath = Path.Combine(uploadDir, imageName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HINH_ANH.CopyToAsync(stream);
                    }

                    nguoiDungModel.HINH_ANH = imageName;
                }

                // Set default values
                nguoiDungModel.VAI_TRO = 2;
                nguoiDungModel.NGAY_TAO = DateTime.Now;
                nguoiDungModel.TRANG_THAI = 0;

                _dataContext.Add(nguoiDungModel);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Thêm khách hàng thành công!";
                return RedirectToAction(nameof(NhanVienAdmin));
            }
            TempData["ThatBai"] = "Thêm khách hàng thất bại!";
            return View(nguoiDungModel);
        }

        // GET: NhanVienAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDungModel = await _dataContext.NGUOI_DUNGs.FindAsync(id);
            if (nguoiDungModel == null)
            {
                return NotFound();
            }
            return View(nguoiDungModel);
        }

        // POST: NhanVienAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NguoiDungModel nguoiDungModel, IFormFile HINH_ANH)
        {
            if (id != nguoiDungModel.ID_NGUOI_DUNG)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle file upload
                    if (HINH_ANH != null && HINH_ANH.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(HINH_ANH.FileName).ToLowerInvariant();

                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("HINH_ANH", "Chỉ chấp nhận các định dạng ảnh: .jpg, .jpeg, .png");
                            return View(nguoiDungModel);
                        }

                        var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/NhanVien1");
                        var imageName = Guid.NewGuid().ToString() + "_" + HINH_ANH.FileName;
                        var filePath = Path.Combine(uploadDir, imageName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HINH_ANH.CopyToAsync(stream);
                        }

                        nguoiDungModel.HINH_ANH = imageName;
                    }

                    _dataContext.Update(nguoiDungModel);
                    await _dataContext.SaveChangesAsync();
                    TempData["ThanhCong"] = "Cập nhật nhân viên thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoiDungModelExists(nguoiDungModel.ID_NGUOI_DUNG))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(NhanVienAdmin));
            }
            TempData["ThatBai"] = "Cập nhật nhân viên thất bại!";
            return View(nguoiDungModel);
        }

        // GET: NhanVienAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDungModel = await _dataContext.NGUOI_DUNGs
                .FirstOrDefaultAsync(m => m.ID_NGUOI_DUNG == id);
            if (nguoiDungModel == null)
            {
                return NotFound();
            }

            return View(nguoiDungModel);
        }

        // POST: NhanVienAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nguoiDungModel = await _dataContext.NGUOI_DUNGs.FindAsync(id);
            if (nguoiDungModel == null)
            {
                TempData["ThatBai"] = "Nhân viên không tồn tại!";
                return RedirectToAction(nameof(NhanVienAdmin));
            }

            _dataContext.NGUOI_DUNGs.Remove(nguoiDungModel);
            await _dataContext.SaveChangesAsync();
            TempData["ThanhCong"] = "Xóa nhân viên thành công!";
            return RedirectToAction(nameof(NhanVienAdmin));
        }

        private bool NguoiDungModelExists(int id)
        {
            return _dataContext.NGUOI_DUNGs.Any(e => e.ID_NGUOI_DUNG == id);
        }
    }
}
