using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBanThatLung.Models;
using WebBanThatLung.Repositoty;

namespace WebBanThatLung.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            var sanPham = _dataContext.SAN_PHAMs
                             .Include(sp => sp.HINH_ANH)
                             .Take(8)
                             .ToList();
            return View(sanPham);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> ThemGioHang()
        {


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
