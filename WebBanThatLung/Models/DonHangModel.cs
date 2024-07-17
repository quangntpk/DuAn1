using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThatLung.Models
{
    public class DonHangModel
    {
        [Key]
        public int ID_DON_HANG { get; set; }
        public int ID_NGUOI_DUNG { get; set; }
        [ForeignKey("ID_NGUOI_DUNG")]
        public NguoiDungModel NGUOI_DUNG { get; set; }

        public int? ID_NHAN_VIEN { get; set; }
        [ForeignKey("ID_NHAN_VIEN")]
        public NguoiDungModel NHAN_VIEN { get; set; }

        public DateTime NGAY_DAT {  get; set; }
        public int TRANG_THAI_DH { get; set; } = 0;
        public string TRANG_THAI_THANH_THAM { get; set; } = "Chưa thanh toán";
        public string HinhThucThanhToan { get; set; }
        public DateTime? NGAY_GIAO { get; set; }
        public string LY_DO_HUY { get; set; } = "";
        public string TEN_NGUOI_NHAN { get; set; }

        public string DIACHI { get; set; }

        public ICollection<ChiTietDonHangModel> CHI_TIET_DON_HANG { get; set; }


    }
}
