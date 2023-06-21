using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;

namespace P013EStore.WebAPIUsing.Models
{
	public class BrandsController : Controller
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiAdres = "https://localhost:7180/api/Brands";
		public BrandsController(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		// GET: BrandsController
		public async Task<IActionResult> Index(int? id)
		{
			if (id is null)
			{
				return BadRequest();
			}
			var model = await _httpClient.GetFromJsonAsync<Brand>(_apiAdres + id.Value);
			if (model is null)
			{
				return NotFound();
			}
			return View(model);
		}

		// GET: BrandsController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

	}	
}
