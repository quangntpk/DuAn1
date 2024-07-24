﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;
using WebBanThatLung.Repository;
using WebBanThatLung.Repositoty;

namespace WebBanThatLung.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly DataContext _dataContext;

        public NguoiDungController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

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

                if (khachHang != null && khachHang.TRANG_THAI == 0)
                {
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
                // Kiểm tra tính duy nhất của các trường
                if (await _dataContext.NGUOI_DUNGs.AnyAsync(nd => nd.TAI_KHOAN == nguoiDung.TAI_KHOAN))
                {
                    ModelState.AddModelError("TAI_KHOAN", "Tài khoản này đã được sử dụng.");
                }

                if (await _dataContext.NGUOI_DUNGs.AnyAsync(nd => nd.SDT == nguoiDung.SDT))
                {
                    ModelState.AddModelError("SDT", "Số điện thoại này đã được sử dụng.");
                }

                if (await _dataContext.NGUOI_DUNGs.AnyAsync(nd => nd.EMAIL == nguoiDung.EMAIL))
                {
                    ModelState.AddModelError("EMAIL", "Email này đã được sử dụng.");
                }
                if (await _dataContext.NGUOI_DUNGs.AnyAsync(nd => nd.CCCD == nguoiDung.CCCD))
                {
                    ModelState.AddModelError("CCCD", "Số căn cước công dân này đã được sử dụng.");
                }

                // Nếu có lỗi, trả lại view cùng với các lỗi đã phát hiện
                if (!ModelState.IsValid)
                {
                    return View(nguoiDung);
                }

                // Nếu không có lỗi, tiến hành đăng ký
                nguoiDung.VAI_TRO = 0;
                nguoiDung.HINH_ANH = "User_images.jpg";
                nguoiDung.NGAY_TAO = DateTime.Now;
                nguoiDung.TRANG_THAI = 0;
                nguoiDung.NGAY_SINH = DateTime.Now;

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
    }
}