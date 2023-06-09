﻿using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Models;

namespace P013EStore.WebAPIUsing.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7180/api/Products";
        [Route("tum-urunlerimiz")]
        public async Task<IActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres);
            return View(model);
        }
		public async Task<IActionResult> DetailAsync(int id)
		{
			var model = new ProductDetailViewModel(); // detailde görünen ürüne ait kategorideki diğer ürünler yayınlansın diye viewmodel oluşturduk.
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres);
			var product = await _httpClient.GetFromJsonAsync<Product>(_apiAdres + "/" + id);
            model.Product = product;
            model.RelatedProducts = products.Where(p => p.CategoryId == product.CategoryId && p.Id != id).ToList();
			if (model is null)
			{
				return NotFound();
			}
			return View(model);
		}
		public async Task<IActionResult> Search(string q) // adres çubuğunda query string ile 
		{
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres + "/GetSearch/" + q);
			
			return View(products);
		}
	}
}
