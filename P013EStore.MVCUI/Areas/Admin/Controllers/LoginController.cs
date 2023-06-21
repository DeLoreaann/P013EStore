using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Models;
using P013EStore.Service.Abstract;
using System.Security.Claims;

namespace P013EStore.MVCUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IService<AppUser> _service;

        public LoginController(IService<AppUser> service)
        {
            _service = service;
        }
        public IActionResult Index(string ReturnUrl)
        {
            var model = new AdminLoginViewModel();
            model.ReturnUrl = ReturnUrl;
            return View(model);
        }
        [Route("Logout")] // logout url isteği gelirse bu action çalışacak.
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // sistemden çıkış yap
            return Redirect("/Admin/Login"); // tekrardan giriş sayfasına yönlendir.
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(AdminLoginViewModel admin)
        {
            try
            {
                var kullanici = await _service.GetAsync(k=>k.IsActive && k.Email == admin.Email && k.Password == admin.Password);
                if (kullanici != null)
                {
                    var kullaniciYetkileri = new List<Claim> 
                    {
                        new Claim(ClaimTypes.Email, kullanici.Email),
                        new Claim("Role", kullanici.IsAdmin ? "Admin" : "User"),
                        new Claim("UserGuid", kullanici.UserGuid.ToString())
                    };
                    var kullaniciKimligi = new ClaimsIdentity(kullaniciYetkileri, "Login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(kullaniciKimligi);
                    await HttpContext.SignInAsync(principal);
                    return Redirect(string.IsNullOrEmpty(admin.ReturnUrl) ? "/Admin/Main" : admin.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Giriş Başarısız!");
                }
            }
            catch (Exception hata)
            {
                ModelState.AddModelError("", "Hata oluştu!" + hata.Message);
            }
            return View();
        }
        
    }
}
