using Microsoft.AspNetCore.Mvc;
using WebBanThatLung.Models;
using System.Threading.Tasks;
using WebBanThatLung.Services;

namespace WebBanThatLung.Controllers
{
    public class LienHeController : Controller
    {
        private readonly EmailService _emailService;


        public LienHeController(EmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult LienHe()
        {

            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(LienHeModel model)
        {
            if (ModelState.IsValid)
            {
                var subject = $"Thông điệp liên hệ từ {model.Name}";
                var body = $"<p><strong>Họ tên:</strong> {model.Name}</p>" +
                           $"<p><strong>Email:</strong> {model.Email}</p>" +
                           $"<p><strong>Điện thoại:</strong> {model.Phone}</p>" +
                           $"<p><strong>Lời nhắn:</strong> {model.Message}</p>";

                try
                {
                    await _emailService.SendEmailAsync("champhan625@gmail.com", subject, body);
                    TempData["ThanhCong"] = "Thông điệp của bạn đã được gửi thành công.";
                }
                catch (Exception)
                {

                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi gửi thông điệp. Vui lòng thử lại sau.";
                }

                return RedirectToAction("LienHe");
            }

            return View("LienHe", model);
        }
    }
}
