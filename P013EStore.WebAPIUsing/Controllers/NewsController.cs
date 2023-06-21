using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;

namespace P013EStore.WebAPIUsing.Controllers
{
	public class NewsController : Controller
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiAdres = "https://localhost:7180/api/News";
		public NewsController(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<ActionResult> Index()
		{
			var model = await _httpClient.GetFromJsonAsync<List<News>>(_apiAdres);
			return View(model);
		}
	}
}
