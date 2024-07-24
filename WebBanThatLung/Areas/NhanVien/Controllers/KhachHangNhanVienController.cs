using Microsoft.AspNetCore.Mvc;

namespace WebBanThatLung.Areas.NhanVien.Controllers
{

    [Area("NhanVien")]

    public class KhachHangNhanVienController : Controller
    {

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
    }
}
