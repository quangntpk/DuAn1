using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;
using WebBanThatLung.Repository;
using WebBanThatLung.Repositoty; // Chú ý kiểm tra xem namespace có bị trùng lặp không
using System.Net.Mail;
using System.Net;

namespace WebBanThatLung.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly DataContext _dataContext;

        public NguoiDungController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string TAI_KHOAN, string MAT_KHAU)
        {
            if (ModelState.IsValid)
            {
                var khachHang = await _dataContext.NGUOI_DUNGs
                    .FirstOrDefaultAsync(kh => kh.TAI_KHOAN == TAI_KHOAN && kh.MAT_KHAU == MAT_KHAU);

                if (khachHang != null)
                {
                    if (khachHang.LockoutEndDate.HasValue && khachHang.LockoutEndDate.Value > DateTime.Now)
                    {
                        TempData["Error"] = $"Tài khoản của bạn đã bị khóa đến {khachHang.LockoutEndDate.Value.ToString("dd/MM/yyyy HH:mm:ss")}.";
                        return View();
                    }

                    khachHang.CancelCount = 0;
                    khachHang.LockoutEndDate = null;
                    _dataContext.Update(khachHang);
                    await _dataContext.SaveChangesAsync();

                    HttpContext.Session.SetJson("User", khachHang);
                    TempData["ThanhCong"] = "Đăng nhập thành công";

                    if (khachHang.VAI_TRO == 1)
                    {
                        return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                    }
                    else if (khachHang.VAI_TRO == 2)
                    {
                        return RedirectToAction("Index", "HomeNhanVien", new { area = "NhanVien" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên tài khoản hoặc mật khẩu không đúng.");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(NguoiDungModel nguoiDung)
        {
            if (ModelState.IsValid)
            {
                if (await _dataContext.NGUOI_DUNGs.AnyAsync(nd => nd.TAI_KHOAN == nguoiDung.TAI_KHOAN))
                {
                    ModelState.AddModelError("TAI_KHOAN", "Tài khoản này đã được sử dụng.");
                    return View(nguoiDung);
                }

                if (await _dataContext.NGUOI_DUNGs.AnyAsync(nd => nd.SDT == nguoiDung.SDT))
                {
                    ModelState.AddModelError("SDT", "Số điện thoại này đã được sử dụng.");
                    return View(nguoiDung);
                }

                if (await _dataContext.NGUOI_DUNGs.AnyAsync(nd => nd.CCCD == nguoiDung.CCCD))
                {
                    ModelState.AddModelError("CCCD", "Số căn cước công dân này đã được sử dụng.");
                    return View(nguoiDung);
                }

                nguoiDung.VAI_TRO = 0;
                nguoiDung.HINH_ANH = "User_images.jpg";
                nguoiDung.NGAY_TAO = DateTime.Now;
                nguoiDung.TRANG_THAI = 0;

                _dataContext.Add(nguoiDung);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Đăng ký thành công";
                return RedirectToAction(nameof(Login));
            }

            return View(nguoiDung);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            TempData["ThanhCong"] = "Đăng xuất thành công";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _dataContext.NGUOI_DUNGs
                    .FirstOrDefaultAsync(u => u.EMAIL == model.Email);

                if (user == null)
                {
                    TempData["ThatBai"] = "Email không đúng.";
                }
                else
                {
                    // Sinh mã OTP
                    string otp = GenerateOTP(6);

                    // Lưu OTP và thời gian hết hạn vào Session
                    HttpContext.Session.SetString("UserEmail", user.EMAIL);
                    HttpContext.Session.SetString("OTP", otp);
                    HttpContext.Session.SetString("OTPExpiration", DateTime.Now.AddMinutes(10).ToString());

                    // Cấu hình gửi email
                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential("champhan625@gmail.com", "ejguitygsfnvpwyt"),
                        EnableSsl = true,
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("champhan625@gmail.com"),
                        Subject = "Mã OTP Đặt lại mật khẩu",
                        Body = $"Mã OTP của bạn là: {otp}\nHãy sử dụng mã này trong vòng 10 phút.",
                        IsBodyHtml = false,
                    };
                    mailMessage.To.Add(user.EMAIL);

                    try
                    {
                        await smtpClient.SendMailAsync(mailMessage);
                        TempData["ThanhCong"] = "Một mã OTP đã được gửi đến email của bạn.";
                    }
                    catch (SmtpException smtpEx)
                    {
                        TempData["ThatBai"] = "Có lỗi SMTP xảy ra khi gửi email: " + smtpEx.Message;
                    }
                    catch (Exception ex)
                    {
                        TempData["ThatBai"] = "Có lỗi xảy ra khi gửi email: " + ex.Message;
                    }
                }
            }

            return View(model);
        }

        // Phương thức sinh mã OTP
        private string GenerateOTP(int length)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string code, string matKhauMoi, string xacNhanMatKhauMoi)
        {
            if (matKhauMoi != xacNhanMatKhauMoi)
            {
                TempData["ThatBai"] = "Mật khẩu xác nhận không khớp.";
                return RedirectToAction("ForgotPassword"); // Điều hướng về trang nhập lại thông tin
            }

            // Lấy mã OTP và thời gian hết hạn từ Session
            var storedOtp = HttpContext.Session.GetString("OTP");
            var storedEmail = HttpContext.Session.GetString("UserEmail");
            var expiration = HttpContext.Session.GetString("OTPExpiration");

            if (storedOtp == null || storedEmail == null || expiration == null)
            {
                TempData["ThatBai"] = "Thông tin xác thực không hợp lệ.";
                return RedirectToAction("ForgotPassword");
            }

            if (code != storedOtp || DateTime.Parse(expiration) <= DateTime.Now)
            {
                TempData["ThatBai"] = "Mã OTP không hợp lệ hoặc đã hết hạn.";
                return RedirectToAction("ForgotPassword");
            }

            var user = await _dataContext.NGUOI_DUNGs
                .FirstOrDefaultAsync(u => u.EMAIL == storedEmail);

            if (user == null)
            {
                TempData["ThatBai"] = "Người dùng không tồn tại.";
                return RedirectToAction("ForgotPassword");
            }

            // Cập nhật mật khẩu mới (không mã hóa mật khẩu)
            user.MAT_KHAU = matKhauMoi;
            _dataContext.Update(user);
            await _dataContext.SaveChangesAsync();

            // Xóa thông tin OTP khỏi Session
            HttpContext.Session.Remove("OTP");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("OTPExpiration");

            TempData["ThanhCong"] = "Mật khẩu của bạn đã được đổi thành công.";
            return RedirectToAction("Login");
        }



        [HttpGet]
        public async Task<IActionResult> ThongTinCaNhan()
        {
            var user = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var userInfo = await _dataContext.NGUOI_DUNGs
                .FirstOrDefaultAsync(u => u.ID_NGUOI_DUNG == user.ID_NGUOI_DUNG);

            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThayDoiMatKhau(string MatKhauCu, string MatKhauMoi, string XacNhanMatKhauMoi)
        {
            var user = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var userInfo = await _dataContext.NGUOI_DUNGs
                .FirstOrDefaultAsync(u => u.ID_NGUOI_DUNG == user.ID_NGUOI_DUNG);

            if (userInfo == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy người dùng.");
                return View("TrangCaNhan");
            }

            bool isPasswordValid = userInfo.MAT_KHAU == MatKhauCu;
            bool isNewPasswordValid = MatKhauMoi == XacNhanMatKhauMoi;
            bool isNewPasswordLengthValid = MatKhauMoi.Length >= 6;

            if (!isPasswordValid)
            {
                TempData["ThatBai"] = "Mật khẩu cũ không đúng.";
            }
            else if (!isNewPasswordValid)
            {
                TempData["ThatBai"] = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
            }
            else if (!isNewPasswordLengthValid)
            {
                TempData["ThatBai"] = "Mật khẩu mới phải có ít nhất 6 ký tự.";
            }
            else
            {
                userInfo.MAT_KHAU = MatKhauMoi;

                try
                {
                    _dataContext.Update(userInfo);
                    await _dataContext.SaveChangesAsync();
                    TempData["ThanhCong"] = "Đã thay đổi mật khẩu thành công.";
                    return RedirectToAction("TrangCaNhan");
                }
                catch (Exception ex)
                {
                    TempData["ThatBai"] = "Có lỗi xảy ra khi thay đổi mật khẩu: " + ex.Message;
                }
            }

            return View("TrangCaNhan", userInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThongTinCaNhan(NguoiDungModel modelNguoiDung, IFormFile? HinhAnhTaiLen)
        {
            var user = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var userInfo = await _dataContext.NGUOI_DUNGs
                .FirstOrDefaultAsync(u => u.ID_NGUOI_DUNG == user.ID_NGUOI_DUNG);

            if (userInfo == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy người dùng.");
                return View(modelNguoiDung);
            }

            userInfo.HO_TEN = modelNguoiDung.HO_TEN;
            userInfo.NGAY_SINH = modelNguoiDung.NGAY_SINH;
            userInfo.SDT = modelNguoiDung.SDT;
            userInfo.EMAIL = modelNguoiDung.EMAIL;
            userInfo.CCCD = modelNguoiDung.CCCD;

            if (HinhAnhTaiLen != null && HinhAnhTaiLen.Length > 0)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "media", "NhanVien", HinhAnhTaiLen.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await HinhAnhTaiLen.CopyToAsync(stream);
                }

                userInfo.HINH_ANH = HinhAnhTaiLen.FileName;
            }

            try
            {
                _dataContext.Update(userInfo);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Đã cập nhật thông tin thành công.";
                return RedirectToAction("ThongTinCaNhan");
            }
            catch (Exception ex)
            {
                TempData["ThatBai"] = "Có lỗi xảy ra khi cập nhật thông tin: " + ex.Message;
            }

            return View(userInfo);
        }
    }
}
