using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;
using System.Linq;
using System.Threading.Tasks;
using WebBanThatLung.Repositoty;
using WebBanThatLung.Repository;
using WebBanThatLung.Services;
using WebBanThatLung.ViewModels;

namespace WebBanThatLung.Controllers
{
    public class DonHangController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IVnPayService _vnPayService;
        private readonly EmailService _emailService;

        public DonHangController(DataContext dataContext, IVnPayService vnPayService, EmailService emailService)
        {
            _dataContext = dataContext;
            _vnPayService = vnPayService;
            _emailService = emailService;
        }

        public async Task<IActionResult> DatHang()
        {
            var khachHang = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (khachHang == null)
            {
                return RedirectToAction("Login", "KhachHang");
            }

            var gioHang = await _dataContext.GIO_HANGs
                                            .Include(gh => gh.SAN_PHAM)
                                            .ThenInclude(sp => sp.HINH_ANH)
                                            .Where(gh => gh.ID_NGUOI_DUNG == khachHang.ID_NGUOI_DUNG)
                                            .ToListAsync();

            if (gioHang == null || gioHang.Count == 0)
            {
                TempData["ThatBai"] = "Giỏ hàng đang trống hãy thêm sản phẩm vào giỏ hàng trước";
                return RedirectToAction("TrangGioHang", "GioHang");
            }

            ViewBag.GioHang = gioHang;
            ViewBag.TongTien = gioHang.Sum(gh => gh.THANH_TIEN);

            return View(khachHang);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatHang(string diaChiGiaoHang, string HoVaTen, string SoDienThoai, string hinhThucThanhToan)
        {
            var khachHang = HttpContext.Session.GetJson<NguoiDungModel>("User");
            if (khachHang == null)
            {
                return RedirectToAction("Login", "KhachHang");
            }

            if (hinhThucThanhToan == null)
            {
                TempData["ThatBai"] = "Vui lòng chọn 1 hình thức thanh toán";
            }

            var gioHang = await _dataContext.GIO_HANGs
                                            .Include(gh => gh.SAN_PHAM)

                                            .Where(gh => gh.ID_NGUOI_DUNG == khachHang.ID_NGUOI_DUNG)
                                            .ToListAsync();



            if (gioHang == null || gioHang.Count == 0)
            {
                TempData["ThaiBai"] = "Giỏ hàng trống.";
                return RedirectToAction("GioHang", "GioHang");
            }

            var donHang = new DonHangModel
            {
                ID_NGUOI_DUNG = khachHang.ID_NGUOI_DUNG,
                DIACHI = diaChiGiaoHang,
                TEN_NGUOI_NHAN = HoVaTen,
                HinhThucThanhToan = hinhThucThanhToan,
                NGAY_DAT = DateTime.Now

            };



            _dataContext.DON_HANGs.Add(donHang);
            await _dataContext.SaveChangesAsync();

            foreach (var item in gioHang)
            {
                var chiTietDonHang = new ChiTietDonHangModel
                {
                    ID_DON_HANG = donHang.ID_DON_HANG,
                    ID_SAN_PHAM = item.ID_SAN_PHAM,

                    SO_LUONG = item.SO_LUONG_GH,
                    GIA = item.SAN_PHAM.GIA,
                    THANH_TIEN = item.THANH_TIEN
                };

                _dataContext.CHI_TIET_DON_HANGs.Add(chiTietDonHang);
            }

            await _dataContext.SaveChangesAsync();

            _dataContext.GIO_HANGs.RemoveRange(gioHang);
            await _dataContext.SaveChangesAsync();

            if (hinhThucThanhToan == "VNPay")
            {
                decimal TongTien = gioHang.Sum(th => th.THANH_TIEN);
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = Convert.ToDouble(TongTien),
                    CreatedDate = DateTime.Now,
                    Description = $"{khachHang.SDT}",
                    OrderId = new Random().Next(1000, 10000),
                    ProductId = donHang.ID_DON_HANG
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));


            }


            TempData["ThanhCong"] = "Đặt hàng thành công.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Error"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("XuatDonHang", "DonHang");
            }
            var ID = int.Parse(response.OrderId);
            var sp = _dataContext.DON_HANGs.FirstOrDefault(x => x.ID_DON_HANG == ID);
            if (sp != null)
            {
                sp.TRANG_THAI_THANH_THAM = "Đã thanh toán";
              
                _dataContext.SaveChanges();
            }
            int id = sp.ID_DON_HANG;

            var HoaDon = _dataContext.DON_HANGs
                .Include(dh => dh.CHI_TIET_DON_HANG)
                    .ThenInclude(ct => ct.SAN_PHAM)
                .Include(dh => dh.NGUOI_DUNG)
                .FirstOrDefault(dh => dh.ID_DON_HANG == id);
            if (HoaDon == null)
            {
                return NotFound();
            }
            var invoiceHtml = $@"
                <h2>Hóa đơn</h2>
                <hr/>
                <p>Đơn hàng: {response.OrderId}</p>
                <p>Tên khách hàng: {HoaDon.NGUOI_DUNG.HO_TEN}</p>
                <p>Ngày đặt hàng: {HoaDon.NGAY_DAT.ToString("dd/MM/yyyy")}</p>
                <p>Trạng thái đơn hàng: {(HoaDon.TRANG_THAI_DH == 0 ? "Đang xử lý" : "")}</p>
                <p>Trạng thái thanh toán: {(HoaDon.TRANG_THAI_THANH_THAM)}</p>
                <p>Hình thức thanh toán: {(HoaDon.HinhThucThanhToan)}</p>
                <p><strong>Chi tiết đơn hàng:</strong></p>
                <ul>";

            foreach (var chiTiet in HoaDon.CHI_TIET_DON_HANG)
            {
                invoiceHtml += $@"
                    <li>
                        Sản phẩm: {chiTiet.SAN_PHAM.TEN_SAN_PHAM} - Số lượng: {chiTiet.SO_LUONG} - Thành tiền: {chiTiet.THANH_TIEN.ToString("N0")} đ
                    </li>";
            }

            invoiceHtml += "</ul>";

            try
            {
                _emailService.SendEmailAsync(HoaDon.NGUOI_DUNG.EMAIL, "Team-two", invoiceHtml);
                TempData["ThanhCong"] = "Gửi hóa đơn qua email thành công.";
            }
            catch (Exception ex)
            {
                return BadRequest("Gửi hóa đơn qua email không thành công.");
            }



            TempData["ThanhCong"] = $"Thanh toán VN Pay thành công";
            return RedirectToAction("Index", "Home");

        }

    }
}
