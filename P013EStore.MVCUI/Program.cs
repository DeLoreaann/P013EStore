using P013EStore.Data;
using P013EStore.Service.Concrete;
using P013EStore.Service.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(); // uygulamada session kullanabilmek için 

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddTransient(typeof(IService<>), typeof(Service<>)); // kendi yazdýðýmýz db iþlemlerini yapan servisi .net core da bu þekilde mvc projesine servis olarak tanýtýyoruz ki kullanabilelim.

builder.Services.AddTransient<IProductService, ProductService>(); // product için yazdýðýmýz özel servisi uygulamaya tanýttýk. AddTransien yöntemiyle servis eklediðimizde sistem uygulamayý çalýþtýrdýðýnda hazýrda ayný nesne varsa o kullanulur yoksa yeni bir nesne oluþturulup kullanýma sunulur.
builder.Services.AddTransient<ICategoryService, CategoryService>();

//builder.Services.AddSingleton<IProductService, ProductService>(); // bunlarý webapiusing esnasýnda ekledik. add singleton yönmetiyle servis eklediðimizde sistem uygulamayý çalýlþtýrdýðýnda bu nesneden 1 tane üretir ve her istekte ayný nesne gönderilir.
//builder.Services.AddScoped<IProductService, ProductService>(); // bu da web api  using esnasýnda eklendi. AddScoped yöntemiyle servis eklediðimizde sistem uygulamayý çalýþtýrdýðýnda bu nesneye gelen her istek için ayrý ayrý nesneler üretip bunu kullanýma sunar. içeriðin çok dinamik bir þekilde deðiþtiði projelerde kullanýlabilir. döviz altýn fiyatý gibi anlýk deðiþimlerin olduðu projelerde mesela.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login"; // oturum açmayan kullanýcýlarýn giriþ için gönderileceði adres
    x.LogoutPath = "/Admin/Logout";
    x.AccessDeniedPath = "/AccessDenied"; // yetkilendirme ile ekrana eriþim hakký olmayan kullanýcýlarýn gönderileceði sayfa.
    x.Cookie.Name = "Administrator"; // Oluþacak kukinin ismi
    x.Cookie.MaxAge = TimeSpan.FromDays(1); // oluþacak kukinin yaþam süresi.
}); // oturum iþlemleri için

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", p => p.RequireClaim("Role", "Admin")); // admin paneline giriþ yapma yetkisine sahip olanlarý bu kuralla kontrol edeceðiz.
    x.AddPolicy("UserPolicy", p => p.RequireClaim("Role", "User")); // admin dýþýnda yetkilendirme kullanýrsak bu kuralý kullanabiliriz.(siteye üye giriþi yapanlarý ön yüzde bir panele eriþtirme gibi.)
});
   


// uygulama admin paneli için admin yetkilendirme alaný

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // session için 

app.UseAuthentication(); // dikkat! önce UseAuthentication satýrý gelmeli sonra UseAuthorization

app.UseAuthorization();

app.MapControllerRoute(
            name: "admin",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
