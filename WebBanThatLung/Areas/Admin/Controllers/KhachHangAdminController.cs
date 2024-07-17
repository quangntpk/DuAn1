using Microsoft.AspNetCore.Mvc;

namespace WebBanThatLung.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhachHangAdminController : Controller
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
