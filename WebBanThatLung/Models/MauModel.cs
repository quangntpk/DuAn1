using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebBanThatLung.Models
{
    public class MauModel
    {
        [Key]
        public int ID_MAU { get; set; }

        [Required(ErrorMessage = "Không được để trống màu")]
        [MinLength(1, ErrorMessage = "Màu phải từ 2 ký tự.")]
        public string MAU { get; set; }

    }
}
