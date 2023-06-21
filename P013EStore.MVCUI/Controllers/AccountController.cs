using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Models;
using P013EStore.Service.Abstract;
using P013EStore.Service.Concrete;

namespace P013EStore.MVCUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IService<AppUser> _service;

        public AccountController(IService<AppUser> context)
        {
            _service = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("userId"); // userId keyword aşağıdaki loginmetodundan geldi
            if (userId is null)
            {
                TempData["Message"] = "<div class='alert alert-danger'>Lütfen Giriş Yapınız!</div>";
                return RedirectToAction("Login");
            }
            else
            {
                var user = await _service.GetAsync(u => u.Id == userId);
                return View(user);
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(AppUser appUser)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("userId");
                var user = await _service.GetAsync(u => u.Id == userId);
                if (user is not null)
                {
                    user.Name = appUser.Name;
                    user.Surname = appUser.Surname;
                    user.Email = appUser.Email;
                    user.Phone = appUser.Phone;
                    user.Password = appUser.Password;
					_service.Update(user);
					await _service.SaveAsync();
					return RedirectToAction(nameof(Index));
				}
                
            }
            catch
            {
				ModelState.AddModelError("", "Hata Oluştu!");
			}
            return View("Index", appUser);

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            var user = await _service.GetAsync(x => x.Email == model.Email && x.Password == model.Password && x.IsActive);
            if (user == null)
            {
                ModelState.AddModelError("", "Giriş Başarısız");
            }
            else
            {
                HttpContext.Session.SetInt32("userId", user.Id); // burayı yazabilmek için program cs tarafında session kullanacağımızı uygulamaya tanıttık
                HttpContext.Session.SetString("userGuid", user.UserGuid.ToString());
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult LogOut()
        {
            try
            {
                HttpContext.Session.Remove("userId");
                HttpContext.Session.Remove("userGuid");
            }
            catch (Exception)
            {
                HttpContext.Session.Clear();
            }
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public async Task<IActionResult> SignInAsync(AppUser appUser)
        {
            try
            {
                var kullanici = await _service.GetAsync(x => x.Email == appUser.Email);
                
                if (kullanici != null)
                {
                    ModelState.AddModelError("", "Bu Mail Kayıtlı!");
                    return View();
                }
                else
                {
                    appUser.UserGuid = Guid.NewGuid();
                    appUser.IsActive = true;
                    appUser.IsAdmin = false;
                    await _service.AddAsync(appUser);
                    var sonuc = await _service.SaveAsync();
                    TempData["Message"] = "<div class='alert alert-success'>Kayıt Başarılı! Giriş Yapabilirsiniz..</div>";
                    if (sonuc > 0)
                    {
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hata oluştu!");
            }
            return View();
        }
        public IActionResult NewPassword()
        {
            return View();
        }
    }
}
