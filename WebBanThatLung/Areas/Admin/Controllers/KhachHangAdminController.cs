using Microsoft.AspNetCore.Mvc;
using WebBanThatLung.Models;
using WebBanThatLung.Repository;
using WebBanThatLung.Repositoty;

namespace WebBanThatLung.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhachHangAdminController : Controller
    {
        private readonly DataContext _dataContext;

        public KhachHangAdminController(DataContext dataContext)
        {
            _dataContext = dataContext;
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
        public IActionResult ChinhSuaThongTin(string HoTen, string SoDienThoai, string email, string diaChi)
        {
            var NguoiDung = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (NguoiDung == null)
            {
                TempData["Loi"] = "Bạn cần đăng nhập để cập nhật thông tin.";
                return RedirectToAction("Login", "NguoiDung", new { area = "" });
            }

            var userToUpdate = _dataContext.NGUOI_DUNGs.FirstOrDefault(s => s.ID_NGUOI_DUNG == NguoiDung.ID_NGUOI_DUNG);
            if (userToUpdate == null)
            {
                TempData["Loi"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Index");
            }

            userToUpdate.HO_TEN = HoTen;
            userToUpdate.SDT = SoDienThoai;
            userToUpdate.EMAIL = email;


            _dataContext.SaveChanges();
            HttpContext.Session.SetJson("User", userToUpdate);

            TempData["ThanhCong"] = "Thông tin đã được cập nhật thành công";
            return RedirectToAction("TrangCaNhan");
        }




    }



   
}
