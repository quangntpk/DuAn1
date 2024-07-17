using WebBanThatLung.Models;

namespace WebBanThatLung.ViewModels
{
    public class LoaiSanPhamViewModel
    {
        public IEnumerable<LoaiSanPhamModel> LoaiSanPhams { get; set; }
        public LoaiSanPhamModel LoaiSanPham { get; set; }
    }
}
