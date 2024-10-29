using System.ComponentModel.DataAnnotations;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Mật khẩu cũ là bắt buộc.")]
    public string MatKhauCu { get; set; }

    [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
    public string MatKhauMoi { get; set; }

    [Required(ErrorMessage = "Xác nhận mật khẩu mới là bắt buộc.")]
    [Compare("MatKhauMoi", ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.")]
    public string XacNhanMatKhauMoi { get; set; }
}

