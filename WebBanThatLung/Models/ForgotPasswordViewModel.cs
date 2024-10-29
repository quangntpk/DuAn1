using System.ComponentModel.DataAnnotations;

namespace WebBanThatLung.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Yêu cầu nhập Email")]
        [EmailAddress(ErrorMessage = "Định dạng Email không đúng")]
        public string Email { get; set; }
    }


}
