using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Models;
using P013EStore.MVCUI.Utils;
using P013EStore.Service.Abstract;
using System.Diagnostics;

namespace P013EStore.MVCUI.Controllers
{
	public class HomeController : Controller
	{
		private readonly IService<Slider> _serviceSlider;
		private readonly IService<Product> _serviceProduct;
		private readonly IService<Contact> _serviceContact;
        private readonly IService<News> _serviceNews;
        private readonly IService<Brand> _serviceBrand;
		private readonly IService<AppLog> _serviceLog;
        private readonly IService<AppLog> _serviceSettings;
        public HomeController(IService<Slider> serviceSlider, IService<Product> serviceProduct, IService<Contact> serviceContact, IService<News> serviceNews, IService<Brand> serviceBrand, IService<AppLog> serviceLog, IService<AppLog> serviceSettings)
        {
            _serviceSlider = serviceSlider;
            _serviceProduct = serviceProduct;
            _serviceContact = serviceContact;
            _serviceNews = serviceNews;
            _serviceBrand = serviceBrand;
            _serviceLog = serviceLog;
            _serviceSettings = serviceSettings;
        }

        public async Task<IActionResult> Index()
		{
			var model = new HomePageViewModel()
			{
				Sliders = await _serviceSlider.GetAllAsync(),
				Products = await _serviceProduct.GetAllAsync(p => p.IsActive && p.IsHome),
				Brands = await _serviceBrand.GetAllAsync(b => b.IsActive),
				News = await _serviceNews.GetAllAsync(n => n.IsActive && n.IsHome)
			};
			return View(model);
		}

		public async Task<IActionResult> Privacy()
		{
			return View();
		}
		[Route("iletisim")]
		public async Task<IActionResult> ContactUs()
		{
			var model = await _serviceSettings.GetAllAsync();
			if (model is not null)
			{
				return View(model.FirstOrDefault());
			}
			return View();
		}
		[Route("iletisim"), HttpPost]
		public async Task<IActionResult> ContactUsAsync(Contact contact)
		{
			if (ModelState.IsValid)
			{
				try
				{
					
					await _serviceContact.AddAsync(contact);
					var sonuc = await _serviceContact.SaveAsync();
					if (sonuc > 0)
					{
						//MailHelper.SendMailAsync(contact); // gelen mesajı mail gönderir
						TempData["Message"] = "<div class='alert alert-success'> Mesajınız Gönderildi! Teşekkürler</div>";
						return RedirectToAction("ContactUs");
					}
				}
				catch (Exception hata)
				{
					_serviceLog.Add(new AppLog
					{
						Title = "İletişim Formu Gönderilirken Hata Oluştu!",
						Description = hata.Message
					});
					await _serviceLog.SaveAsync();
					//await MailHelper.SendMailAsync(contact); // oluşan hatayı yazılımcıya mail gönder.
					ModelState.AddModelError("", "Hata oluştu!");
				}
			}
			return View(contact);
		}
		[Route("AccesDenied")]
		public IActionResult AccessDenied()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}