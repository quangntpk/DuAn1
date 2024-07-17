using Microsoft.AspNetCore.Mvc;

namespace WebBanThatLung.Areas.Admin.Controllers
{
    public class NhanVienAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
