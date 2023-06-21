using P013EStore.Data;
using P013EStore.Service.Concrete;
using P013EStore.Service.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(); // uygulamada session kullanabilmek i�in 

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddTransient(typeof(IService<>), typeof(Service<>)); // kendi yazd���m�z db i�lemlerini yapan servisi .net core da bu �ekilde mvc projesine servis olarak tan�t�yoruz ki kullanabilelim.

builder.Services.AddTransient<IProductService, ProductService>(); // product i�in yazd���m�z �zel servisi uygulamaya tan�tt�k. AddTransien y�ntemiyle servis ekledi�imizde sistem uygulamay� �al��t�rd���nda haz�rda ayn� nesne varsa o kullanulur yoksa yeni bir nesne olu�turulup kullan�ma sunulur.
builder.Services.AddTransient<ICategoryService, CategoryService>();

//builder.Services.AddSingleton<IProductService, ProductService>(); // bunlar� webapiusing esnas�nda ekledik. add singleton y�nmetiyle servis ekledi�imizde sistem uygulamay� �al�l�t�rd���nda bu nesneden 1 tane �retir ve her istekte ayn� nesne g�nderilir.
//builder.Services.AddScoped<IProductService, ProductService>(); // bu da web api  using esnas�nda eklendi. AddScoped y�ntemiyle servis ekledi�imizde sistem uygulamay� �al��t�rd���nda bu nesneye gelen her istek i�in ayr� ayr� nesneler �retip bunu kullan�ma sunar. i�eri�in �ok dinamik bir �ekilde de�i�ti�i projelerde kullan�labilir. d�viz alt�n fiyat� gibi anl�k de�i�imlerin oldu�u projelerde mesela.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login"; // oturum a�mayan kullan�c�lar�n giri� i�in g�nderilece�i adres
    x.LogoutPath = "/Admin/Logout";
    x.AccessDeniedPath = "/AccessDenied"; // yetkilendirme ile ekrana eri�im hakk� olmayan kullan�c�lar�n g�nderilece�i sayfa.
    x.Cookie.Name = "Administrator"; // Olu�acak kukinin ismi
    x.Cookie.MaxAge = TimeSpan.FromDays(1); // olu�acak kukinin ya�am s�resi.
}); // oturum i�lemleri i�in

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", p => p.RequireClaim("Role", "Admin")); // admin paneline giri� yapma yetkisine sahip olanlar� bu kuralla kontrol edece�iz.
    x.AddPolicy("UserPolicy", p => p.RequireClaim("Role", "User")); // admin d���nda yetkilendirme kullan�rsak bu kural� kullanabiliriz.(siteye �ye giri�i yapanlar� �n y�zde bir panele eri�tirme gibi.)
});
   


// uygulama admin paneli i�in admin yetkilendirme alan�

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // session i�in 

app.UseAuthentication(); // dikkat! �nce UseAuthentication sat�r� gelmeli sonra UseAuthorization

app.UseAuthorization();

app.MapControllerRoute(
            name: "admin",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
