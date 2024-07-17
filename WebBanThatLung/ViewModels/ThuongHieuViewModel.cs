using WebBanThatLung.Models;

namespace WebBanThatLung.ViewModels
{
    public class ThuongHieuViewModel
    {
        public IEnumerable<ThuongHieuModel> DanhSachThuongHieu { get; set; }
        public ThuongHieuModel ThuongHieu { get; set; }
    }
}
