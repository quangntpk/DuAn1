using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanThatLung.Models;
using WebBanThatLung.Repository;
using System.IO;
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
        public IActionResult Index()
        {
            var DanhSachSanPham = _dataContext.SAN_PHAMs.Include(s => s.HINH_ANH).ToList();
            return View(DanhSachSanPham);
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
        public async Task<IActionResult> Create(SanPhamModel sanPham, List<IFormFile> HinhAnhTaiLen)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Add(sanPham);
                await _dataContext.SaveChangesAsync();



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


                        HinhAnhModel anh = new HinhAnhModel();
                        anh.TEN_HINH_ANH = imageName;
                        anh.ID_SAN_PHAM = sanPham.ID_SAN_PHAM;
                        _dataContext.HINH_ANHs.Add(anh);
                        _dataContext.SaveChanges();
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
            var SanPham = _dataContext.SAN_PHAMs.Include(h => h.HINH_ANH).FirstOrDefault(sp => sp.ID_SAN_PHAM == id);
            if (SanPham == null)
            {
                return NotFound();
            }

            ViewBag.Loais = new SelectList(_dataContext.LOAI_SAN_PHAMs, "ID_LOAI_SAN_PHAM", "TEN_LOAI_SAN_PHAM");
            ViewBag.ThuongHieus = new SelectList(_dataContext.THUONG_HIEUs, "ID_THUONG_HIEU", "TEN_THUONG_HIEU");
            ViewBag.MAU = new SelectList(_dataContext.MAUs, "ID_MAU", "MAU");

            return View(SanPham);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SanPhamModel SanPham, List<IFormFile> hinhAnhTaiLen, int[] selectedImages)
        {
            if (ModelState.IsValid)
            {
                var existingSanPham = await _dataContext.SAN_PHAMs.Include(h => h.HINH_ANH).FirstOrDefaultAsync(sp => sp.ID_SAN_PHAM == id);
                if (existingSanPham == null)
                {
                    return NotFound();
                }

                existingSanPham.TEN_SAN_PHAM = SanPham.TEN_SAN_PHAM;
                existingSanPham.ID_LOAI_SAN_PHAM = SanPham.ID_LOAI_SAN_PHAM;
                existingSanPham.ID_THUONG_HIEU = SanPham.ID_THUONG_HIEU;
                existingSanPham.ID_MAU = SanPham.ID_MAU;
                existingSanPham.GIA = SanPham.GIA;
                existingSanPham.SO_LUONG = SanPham.SO_LUONG;
                existingSanPham.MO_TA = SanPham.MO_TA;

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

                        HinhAnhModel anh = new HinhAnhModel();
                        anh.TEN_HINH_ANH = imageName;
                        anh.ID_SAN_PHAM = existingSanPham.ID_SAN_PHAM;
                        existingSanPham.HINH_ANH.Add(anh);
                    }
                }

                if (selectedImages != null && selectedImages.Length > 0)
                {
                    foreach (var imageId in selectedImages)
                    {
                        var imageToRemove = existingSanPham.HINH_ANH.FirstOrDefault(img => img.ID_HINH_ANH == imageId);
                        if (imageToRemove != null)
                        {
                            existingSanPham.HINH_ANH.Remove(imageToRemove);

                            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham", imageToRemove.TEN_HINH_ANH);
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }
                    }
                }

                _dataContext.SaveChanges();
                TempData["ThanhCong"] = "Sửa sản phẩm thành công.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Loais = new SelectList(_dataContext.LOAI_SAN_PHAMs, "ID_LOAI_SAN_PHAM", "TEN_LOAI_SAN_PHAM");
            ViewBag.ThuongHieus = new SelectList(_dataContext.THUONG_HIEUs, "ID_THUONG_HIEU", "TEN_THUONG_HIEU");
            ViewBag.MAU = new SelectList(_dataContext.MAUs, "ID_MAU", "MAU");
            return View(SanPham);
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var SanPham = await _dataContext.SAN_PHAMs.FindAsync(id);
            if (SanPham == null)
            {
                return NotFound();
            }

            var hinhAnhs = _dataContext.HINH_ANHs.Where(h => h.ID_SAN_PHAM == id).ToList();
            foreach (var hinhAnh in hinhAnhs)
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "media/SanPham", hinhAnh.TEN_HINH_ANH);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _dataContext.HINH_ANHs.Remove(hinhAnh);
            }

            _dataContext.SAN_PHAMs.Remove(SanPham);
            await _dataContext.SaveChangesAsync();
            TempData["ThanhCong"] = "Xóa sản phẩm thành công";
            return RedirectToAction(nameof(Index));
        }

    }
}


