﻿using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Models;
using System.Diagnostics;

namespace P013EStore.WebAPIUsing.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7180/api/";

        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres + "Products");
            var brands = await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdres + "Brands");

            var model = new HomePageViewModel()
            {
                Sliders = await _httpClient.GetFromJsonAsync<List<Slider>>(_apiAdres + "Sliders"),
                Products = products.Where(p => p.IsActive && p.IsHome).ToList(),
                Brands = brands.Where(p => p.IsActive).ToList(),
                News = await _httpClient.GetFromJsonAsync<List<News>>(_apiAdres + "News")


            };
            return View(model);
        }
        [Route("iletisim")]
        public IActionResult ContactUs()
        {
            return View();
        }
        [Route("iletisim"), HttpPost]
        public async Task<IActionResult> ContactUsAsync(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var response = await _httpClient.PostAsJsonAsync(_apiAdres + "Contacts", contact);
                    if (response.IsSuccessStatusCode)
                    {
                        //MailHelper.SendMailAsync(contact); // gelen mesajı mail gönderir
                        TempData["Message"] = "<div class='alert alert-success'> Mesajınız Gönderildi! Teşekkürler</div>";
                        return RedirectToAction("ContactUs");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata oluştu!");
                }
            }
            return View();
        }
        [Route("AccesDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Privacy()
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