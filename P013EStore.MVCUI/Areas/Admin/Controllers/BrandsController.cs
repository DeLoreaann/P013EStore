using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Utils;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class BrandsController : Controller
    {
        private readonly IService<Brand> _service;

        public BrandsController(IService<Brand> service)
        {
            _service = service;
        }

        // read onlyler sadece constructor metotta doldurulabilir.
        // GET: BrandsController
        public ActionResult Index()
        {
            var model = _service.GetAll();
            return View(model);
        }

        // GET: BrandsController/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: BrandsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Brand collection, IFormFile? logo)
        {
            try
            {
                if (logo is not null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(logo);
                }
                await _service.AddAsync(collection);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        

        // GET: BrandsController/Edit/5
        public async Task<ActionResult> EditAsync(int? id) // id gönderilmeden direkt edit sayfası açılırsa
        {
            if (id == null)
            {
                return BadRequest(); // geriye geçersiz istek hatası dön
            }
            var model = await _service.FindAsync(id.Value); // yukarıdaki id yi ? ile nullable yaparsak
            if (model == null)
            {
                return NotFound(); // kayıt bulunamadı
            }
            return View(model);
        }

        // POST: BrandsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Brand collection, IFormFile? logo, bool? resmiSil)
        {
            try
            {
                if (resmiSil is not null && resmiSil == true)
                {
                    FileHelper.FileRemover(collection.Logo);
                    collection.Logo = "";
                }
                if (logo is not null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(logo);
                }
                _service.Update(collection);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View();
        }

        // GET: BrandsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _service.FindAsync(id);
            return View(model);
        }

        // POST: BrandsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Brand collection)
        {
            try
            {
                FileHelper.FileRemover(collection.Logo);
                _service.Delete(collection);
                _service.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
