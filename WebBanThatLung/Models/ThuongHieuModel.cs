using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanThatLung.Models
{
    public class ThuongHieuModel
    {
        [Key]
        public int ID_THUONG_HIEU { get; set; }

        [Required(ErrorMessage = "Không được để trống tên thương hiệu.")]
        [MinLength(5, ErrorMessage = "Tên thương hiệu phải từ 5 ký tự.")]
        public string TEN_THUONG_HIEU { get; set; }

    }
}
