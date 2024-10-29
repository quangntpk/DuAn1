using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;
using WebBanThatLung.Repository;
using System.IO;
using X.PagedList;
using WebBanThatLung.Repositoty;

namespace WebBanThatLung.Areas.NhanVien.Controllers
{
    [Area("NhanVien")]
    [Route("NhanVien")]
    [Route("NhanVien/HomeNhanVien")]
    public class HomeNhanVienController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeNhanVienController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            var DanhSachSanPham = _dataContext.SAN_PHAMs.Include(s => s.HINH_ANH).Include(m => m.SanPhamMau).ThenInclude(V => V.Mau).AsQueryable();
            var SanPhamTrang = DanhSachSanPham.ToPagedList(page, pageSize);
            return View(SanPhamTrang);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            ViewBag.Loais = new SelectList(_dataContext.LOAI_SAN_PHAMs, "ID_LOAI_SAN_PHAM", "TEN_LOAI_SAN_PHAM");
            ViewBag.ThuongHieus = new SelectList(_dataContext.THUONG_HIEUs, "ID_THUONG_HIEU", "TEN_THUONG_HIEU");
            ViewBag.MAU = new SelectList(_dataContext.MAUs, "ID_MAU", "MAU");
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPhamModel sanPham, List<IFormFile> HinhAnhTaiLen, int[] selectedColors)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Add(sanPham);
                await _dataContext.SaveChangesAsync();

                foreach (var colorId in selectedColors)
                {
                    var sanPhamMau = new SanPhamMauModel
                    {
                        ID_SAN_PHAM = sanPham.ID_SAN_PHAM,
                        ID_MAU = colorId
                    };
                    _dataContext.SAN_PHAM_MAUs.Add(sanPhamMau);
                }

                foreach (var file in HinhAnhTaiLen)
                {
                    if (file != null && file.Length > 0)
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham");
                        string imageName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadDir, imageName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        HinhAnhModel anh = new HinhAnhModel
                        {
                            TEN_HINH_ANH = imageName,
                            ID_SAN_PHAM = sanPham.ID_SAN_PHAM
                        };
                        _dataContext.HINH_ANHs.Add(anh);
                    }
                }

                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Thêm sản phẩm thành công";
                return RedirectToAction(nameof(Index));
            }

            TempData["ThatBai"] = "Thêm sản phẩm thất bại";
            return View(sanPham);
        }


        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var SanPham = _dataContext.SAN_PHAMs.Include(h => h.HINH_ANH).Include(sp => sp.SanPhamMau).FirstOrDefault(sp => sp.ID_SAN_PHAM == id);
            if (SanPham == null)
            {
                return NotFound();
            }

            ViewBag.Loais = new SelectList(_dataContext.LOAI_SAN_PHAMs, "ID_LOAI_SAN_PHAM", "TEN_LOAI_SAN_PHAM");
            ViewBag.ThuongHieus = new SelectList(_dataContext.THUONG_HIEUs, "ID_THUONG_HIEU", "TEN_THUONG_HIEU");
            ViewBag.Maus = _dataContext.MAUs.ToList();

            // Truyền các màu đã chọn
            ViewBag.SelectedColors = SanPham.SanPhamMau.Select(sm => sm.ID_MAU).ToList();

            return View(SanPham);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SanPhamModel SanPham, List<IFormFile> hinhAnhTaiLen, int[] selectedImages, int[] selectedColors)
        {
            if (ModelState.IsValid)
            {
                var existingSanPham = await _dataContext.SAN_PHAMs.Include(h => h.HINH_ANH).Include(sp => sp.SanPhamMau).FirstOrDefaultAsync(sp => sp.ID_SAN_PHAM == id);
                if (existingSanPham == null)
                {
                    return NotFound();
                }

                existingSanPham.TEN_SAN_PHAM = SanPham.TEN_SAN_PHAM;
                existingSanPham.ID_LOAI_SAN_PHAM = SanPham.ID_LOAI_SAN_PHAM;
                existingSanPham.ID_THUONG_HIEU = SanPham.ID_THUONG_HIEU;
                existingSanPham.GIA = SanPham.GIA;
                existingSanPham.SO_LUONG = SanPham.SO_LUONG;
                existingSanPham.MO_TA = SanPham.MO_TA;

                // Cập nhật màu sắc
                existingSanPham.SanPhamMau.Clear();
                foreach (var colorId in selectedColors)
                {
                    var sanPhamMau = new SanPhamMauModel
                    {
                        ID_SAN_PHAM = existingSanPham.ID_SAN_PHAM,
                        ID_MAU = colorId
                    };
                    existingSanPham.SanPhamMau.Add(sanPhamMau);
                }


                foreach (var file in hinhAnhTaiLen)
                {
                    if (file != null && file.Length > 0)
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham");
                        string imageName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadDir, imageName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        HinhAnhModel anh = new HinhAnhModel
                        {
                            TEN_HINH_ANH = imageName,
                            ID_SAN_PHAM = existingSanPham.ID_SAN_PHAM
                        };
                        _dataContext.HINH_ANHs.Add(anh);
                    }
                }

                if (selectedImages != null && selectedImages.Length > 0)
                {
                    var imagesToRemove = existingSanPham.HINH_ANH.Where(h => !selectedImages.Contains(h.ID_HINH_ANH)).ToList();
                    foreach (var image in imagesToRemove)
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham", image.TEN_HINH_ANH);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        _dataContext.HINH_ANHs.Remove(image);
                    }
                }

                _dataContext.Update(existingSanPham);
                await _dataContext.SaveChangesAsync();
                TempData["ThanhCong"] = "Sửa sản phẩm thành công";
                return RedirectToAction(nameof(Index));
            }

            TempData["ThatBai"] = "Sửa sản phẩm thất bại";
            return View(SanPham);
        }


        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var sanPham = await _dataContext.SAN_PHAMs.Include(h => h.HINH_ANH).Include(sp => sp.SanPhamMau).FirstOrDefaultAsync(sp => sp.ID_SAN_PHAM == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            foreach (var image in sanPham.HINH_ANH)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham", image.TEN_HINH_ANH);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _dataContext.SAN_PHAMs.Remove(sanPham);
            await _dataContext.SaveChangesAsync();
            TempData["ThanhCong"] = "Xóa sản phẩm thành công";
            return RedirectToAction(nameof(Index));
        }

    }
}
