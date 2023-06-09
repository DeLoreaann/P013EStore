﻿using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Models;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _serviceProduct;
        private readonly IService<AppLog> _serviceLog;


        public ProductsController(IProductService serviceProduct, IService<AppLog> serviceLog)
        {
            _serviceProduct = serviceProduct;
            _serviceLog = serviceLog;
        }
        [Route("tum-urunlerimiz")] // adres çubuğundan tum-urunlerimiz yazınca bu action çalışsın.
        public async Task<IActionResult> IndexAsync()
        {
            var model = await _serviceProduct.GetAllAsync(p=>p.IsActive);
            return View(model);
        }
		public async Task<IActionResult> Search(string q) 
		{
			var model = await _serviceProduct.GetProductsByIncludeAsync(p => p.IsActive && p.Name.Contains(q) || p.Description.Contains(q) || p.Brand.Name.Contains(q) || p.Category.Name.Contains(q));
			return View(model);
		}
		public async Task<IActionResult> DetailAsync(int id)
        {
            var model = new ProductDetailViewModel(); // detailde görünen ürüne ait kategorideki diğer ürünler yayınlansın diye viewmodel oluşturduk.
            try
            {
                var product = await _serviceProduct.GetProductByIncludeAsync(id);
                model.Product = product;
                model.RelatedProducts = await _serviceProduct.GetAllAsync(p => p.CategoryId == product.CategoryId && p.Id != id); // related ürünleri listeleyebilmek için detay sayfasındaki ürünün kategori id si ile eşleşen ürünleri listeledik. ayrıca detayına baktığımız ürün alaklı ürünler listesinde görünmesin diye && den sonraki kodları yazdık.
            }
            catch (Exception hata)
            {
                var log = new AppLog()
                {
                    Description = "Hata Oluştu : " + hata.Message,
                    Title = "Hata Oluştu"
                };
                await _serviceLog.AddAsync(log);
                await _serviceLog.SaveAsync();
            }
            
            if (model.Product is null)
            {
                return NotFound();
            }
            return View(model);
        }
    }
}
