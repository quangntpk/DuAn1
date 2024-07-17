using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThatLung.Models
{
    public class SanPhamModel
    {
        [Key]
        public int ID_SAN_PHAM { get; set; }

        public string TEN_SAN_PHAM { get; set; }

        public int ID_LOAI_SAN_PHAM { get; set; }
        [ForeignKey("ID_LOAI_SAN_PHAM")]
        public LoaiSanPhamModel? LOAI_SAN_PHAM { get; set; }
        public int ID_THUONG_HIEU { get; set; }
        [ForeignKey("ID_THUONG_HIEU")]
        public ThuongHieuModel? THUONG_HIEU { get; set; }

        public int ID_MAU { get; set; }
        [ForeignKey("ID_MAU")]
        public MauModel? MAU { get; set; }

        public int SO_LUONG { get; set; } 
        public decimal GIA { get; set; }

        public string CHAT_LIEU { get; set; }
        public string MO_TA { get; set; }
        public DateTime NGAY_TAO { get; set; } = DateTime.Now;

        public ICollection<HinhAnhModel>? HINH_ANH { get; set; }

        [NotMapped]
        public IFormFile? HinhAnhTaiLen { get; set; }



    }
}
