using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Utils;

namespace P013EStore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingsControler : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres = "https://localhost:7180/api/Categories";

        public SettingsControler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: SettingsControler
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Settings>>(_apiAdres);
            return View(model);
        }

        // GET: SettingsControler/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SettingsControler/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SettingsControler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Settings collection, IFormFile? Logo, IFormFile? Favicon)
        {
            try
            {
                if (Logo is not null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Logo);
                }
                if (Favicon is not null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Favicon);
                }
                var response = await _httpClient.PostAsJsonAsync(_apiAdres, collection);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(collection);
        }

        // GET: SettingsControler/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Settings>(_apiAdres + "/" + id);
            return View(model);
        }

        // POST: SettingsControler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Settings collection, IFormFile? Logo, IFormFile? Favicon, bool? resmiSil)
        {
            try
            {
                if (resmiSil is not null && resmiSil == true)
                {
                    FileHelper.FileRemover(collection.Logo);
                    collection.Logo = "";
                }
                if (Logo is not null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Logo);
                }
                if (Favicon is not null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Favicon);
                }

                var response = await _httpClient.PutAsJsonAsync(_apiAdres, collection);
                if (response.IsSuccessStatusCode) // apiden başarılı bir istek kodu geldiyse(200 ok)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View();
        }

        // GET: SettingsControler/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Settings>(_apiAdres + "/" + id);
            return View(model);
        }

        // POST: SettingsControler/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Settings collection)
        {
            try
            {
                FileHelper.FileRemover(collection.Logo);
                FileHelper.FileRemover(collection.Favicon);
                await _httpClient.DeleteAsync(_apiAdres + "/" + id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
