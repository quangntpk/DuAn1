using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;
using WebBanThatLung.Repository;
using WebBanThatLung.Repositoty;
using X.PagedList;

namespace WebBanThatLung.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhachHangAdminController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public KhachHangAdminController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;   
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            TempData["ThanhCong"] = "Đăng xuất thành công";
            return RedirectToAction("Login", "NguoiDung", new { area = "" });
        }
        // hiện danh sách khách hàng
        //GET: KhachHangAdmin/ListKhachHang

        [Route("Admin/KhachHangAdmin")]
        public async Task<IActionResult> ListKhachHangAdmin(int page = 1, int pageSize = 10)
        {
            var nguoiDungs = await _dataContext.NGUOI_DUNGs
                .Where(n => n.VAI_TRO == 0)
                .ToListAsync();

            var PhanTrangNhanVien = nguoiDungs.ToPagedList(page, pageSize);

            return View(PhanTrangNhanVien);
        }

        // GET: KhachHangAdmin/Create
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

                    var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/KhachHang");
                    var imageName = Guid.NewGuid().ToString() + "_" + HINH_ANH.FileName;
                    var filePath = Path.Combine(uploadDir, imageName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HINH_ANH.CopyToAsync(stream);
                    }

                    nguoiDungModel.HINH_ANH = imageName;
                }

                // Set default values
                nguoiDungModel.VAI_TRO = 0;
                nguoiDungModel.NGAY_TAO = DateTime.Now;
                nguoiDungModel.TRANG_THAI = 0;

                _dataContext.Add(nguoiDungModel);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Thêm khách hàng thành công!";
                return RedirectToAction(nameof(ListKhachHangAdmin));
            }
            TempData["ThatBai"] = "Thêm khách hàng thất bại!";
            return View(nguoiDungModel);
        }


        // GET: KhachHangAdmin/Edit/5
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

        // GET: KhachHangAdmin/Delete/5
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

        // POST: KhachHangAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nguoiDungModel = await _dataContext.NGUOI_DUNGs.FindAsync(id);
            if (nguoiDungModel == null)
            {
                TempData["ThatBai"] = "Khách hàng không tồn tại!";
                return RedirectToAction(nameof(ListKhachHangAdmin));
            }

            _dataContext.NGUOI_DUNGs.Remove(nguoiDungModel);
            await _dataContext.SaveChangesAsync();
            TempData["ThanhCong"] = "Xóa khách hàng thành công!";
            return RedirectToAction(nameof(ListKhachHangAdmin));
        }

        private bool NguoiDungModelExists(int id)
        {
            return _dataContext.NGUOI_DUNGs.Any(e => e.ID_NGUOI_DUNG == id);
        }


        public IActionResult TrangCaNhan()
        {
            var NguoiDung = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (NguoiDung == null)
            {
                return RedirectToAction("Login", "NguoiDung", new { area = "" });
            }

            var ds = _dataContext.NGUOI_DUNGs.FirstOrDefault(s => s.ID_NGUOI_DUNG == NguoiDung.ID_NGUOI_DUNG);

            if (ds == null)
            {
                TempData["Loi"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Index");
            }

            return View(ds);
        }



        [HttpPost]
        public async Task<IActionResult> ChinhSuaThongTin(string HoTen, string SoDienThoai, string email, string cccd)
        {
            var NguoiDung = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (NguoiDung == null)
            {
                TempData["ThatBai"] = "Bạn cần đăng nhập để cập nhật thông tin.";
                return RedirectToAction("Login", "NguoiDung", new { area = "" });
            }

            var userToUpdate = await _dataContext.NGUOI_DUNGs.FirstOrDefaultAsync(s => s.ID_NGUOI_DUNG == NguoiDung.ID_NGUOI_DUNG);
            if (userToUpdate == null)
            {
                TempData["ThatBai"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Index");
            }

            userToUpdate.HO_TEN = HoTen;
            userToUpdate.SDT = SoDienThoai;
            userToUpdate.EMAIL = email;
            userToUpdate.CCCD = cccd;

            _dataContext.NGUOI_DUNGs.Update(userToUpdate);
            await _dataContext.SaveChangesAsync();
            HttpContext.Session.SetJson("User", userToUpdate);

            TempData["ThanhCong"] = "Thông tin đã được cập nhật thành công";
            return RedirectToAction("TrangCaNhan");
        }


        [HttpPost]
        public async Task<IActionResult> ThayDoiMatKhau(string MatKhauCu, string MatKhauMoi, string XacNhanMatKhauMoi)
        {
            var NguoiDung = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (NguoiDung == null)
            {
                TempData["ThatBai"] = "Bạn cần đăng nhập để thay đổi mật khẩu.";
                return RedirectToAction("Login", "NguoiDung", new { area = "" });
            }

            var userToUpdate = await _dataContext.NGUOI_DUNGs.FirstOrDefaultAsync(s => s.ID_NGUOI_DUNG == NguoiDung.ID_NGUOI_DUNG);
            if (userToUpdate == null)
            {
                TempData["ThatBai"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Index");
            }

            // Centralized error handling
            if (userToUpdate.MAT_KHAU != MatKhauCu)
            {
                TempData["ThatBai"] = "Mật khẩu cũ không chính xác.";
            }
            else if (string.IsNullOrEmpty(MatKhauMoi) || MatKhauMoi.Length < 6)
            {
                TempData["ThatBai"] = "Mật khẩu mới phải có ít nhất 6 ký tự.";
            }
            else if (MatKhauMoi != XacNhanMatKhauMoi)
            {
                TempData["ThatBai"] = "Xác nhận mật khẩu mới không khớp.";
            }
            else
            {
                // Cập nhật mật khẩu
                userToUpdate.MAT_KHAU = MatKhauMoi;
                _dataContext.NGUOI_DUNGs.Update(userToUpdate);
                await _dataContext.SaveChangesAsync();
                HttpContext.Session.SetJson("User", userToUpdate);

                TempData["ThanhCong"] = "Mật khẩu đã được thay đổi thành công.";
                return RedirectToAction("TrangCaNhan");
            }

            return RedirectToAction("TrangCaNhan");
        }
        [HttpPost]
        public async Task<IActionResult> ResetCancelCount(int id)
        {
            var nguoiDung = await _dataContext.NGUOI_DUNGs.FindAsync(id);
            if (nguoiDung != null)
            {
                nguoiDung.CancelCount = 0;

                // Đặt lại thời gian khóa (nếu có)
                nguoiDung.LockoutEndDate = null;

                _dataContext.Update(nguoiDung);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Đặt lại trạng thái tài khoản thành công!";
            }
            else
            {
                TempData["ThatBai"] = "Khách hàng không tồn tại!";
            }

            return RedirectToAction(nameof(ListKhachHangAdmin));
        }

    }
}
