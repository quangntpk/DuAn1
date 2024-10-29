using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThatLung.Models
{
    public class SanPhamMauModel
    {
        [Key]
        public int ID_MauSanPham { get; set; }
        public int ID_SAN_PHAM { get; set; }
        [ForeignKey("ID_SAN_PHAM")]
        public SanPhamModel SanPham { get; set; }

     
        public int ID_MAU { get; set; }
        [ForeignKey("ID_MAU")]
        public MauModel Mau { get; set; }
    }
}
