using Microsoft.AspNetCore.Mvc;

namespace WebBanThatLung.Areas.NhanVien.Controllers
{
    [Area("NhanVien")]
    [Route("NhanVien")]
    [Route("HomeNhanVien")]
    public class HomeNhanVienController : Controller
    {
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
