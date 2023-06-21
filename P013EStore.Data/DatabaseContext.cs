using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using P013EStore.Core.Entities;
using P013EStore.Data.Configurations;
using System.Reflection;

namespace P013EStore.Data
{
	public class DatabaseContext : DbContext
	{
        // katmanlı mimaride bir proje katmanından başka bir katmana erişebilmek için bulunduğumuz data projesinin dependencies kısmına sağ tıklayıp > Add project references diyerek açılan pencereden core projesine tik atıp ok diyerek pencereyi kapatmamız gerekiyor.
        public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<News> News { get; set; }
		public DbSet<AppLog> Logs { get; set; }
		public DbSet<Settings> Settings { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// OnConfiguring metodu entityframework ile gelir ve veritabanı bağlantı ayarlarını yapmamızı sağlar.
			optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; Database=P013EStore; Trusted_Connection=True");
			//optionsBuilder.UseSqlServer(@"Server=CanliServerAdi; Database=CanlidakiDatabase; Username=CanliVeritabaniKullaniciAdi; Password=CanliVeritabaniSifre");
			base.OnConfiguring(optionsBuilder);
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// FluentAPI ile veritabanı tablolarımız oluşurken veri tiplerini db kurallarını burada tanımlayabiliriz.
			modelBuilder.Entity<AppUser>().Property(a => a.Name).IsRequired().HasMaxLength(50); // FluentAPI ile AppUser class ının Name Property si için oluşacak veritabanı kolonu ayarlarını bu ekilde belirtebiliyoruz.   //
			modelBuilder.Entity<AppUser>().Property(a => a.Surname).HasMaxLength(50);
			modelBuilder.Entity<AppUser>().Property(a => a.UserName).HasColumnType("varchar(50)").HasMaxLength(50);
			modelBuilder.Entity<AppUser>().Property(a => a.Password).IsRequired().HasColumnType("nvarchar(100)").HasMaxLength(100);
			modelBuilder.Entity<AppUser>().Property(a => a.Email).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<AppUser>().Property(a => a.Phone).HasMaxLength(20);

			//FluentAPI HasData ile db oluştukta sonra başlangıçkatırları ekleme

			modelBuilder.Entity<AppUser>().HasData(new AppUser
			{
				Id=1,
				Email = "info@P013EStore.com",
				Password = "123",
				UserName = "Admin",
				IsActive = true,
				IsAdmin = true,
				Name = "Admin",
				UserGuid = Guid.NewGuid(), // kullanıcıya benzersiz bir id no oluşturur.
				

			});
			//modelBuilder.ApplyConfiguration(new BrandConfigurations()); // marka için yaptığımız konfigürasyon ayarlarını çağırdık.
			//modelBuilder.ApplyConfiguration(new CategoryConfigurations());
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // uygulamadaki tüm Configurations class larını burada çalıştırır. yukarıda model builder diye başlayan yorum satırlarındaki kodları tek tek tüm entityler için yazmak yerine topluca ekleme yapmak için bu kodu kullanıyoruz.

			// Fluent Validation : data annotationdaki hata mesajları vb işlemlerini yönetebileceğimiz 3. parti paket.

			// Katmanlı mimaride MvcWebUI katmanından direkt data katmanına erişilmesi istenmez. Arada bir iş katmanınınP013EStore.Services tüm bu süreçlerini yönetmesi istenir. Bu yüzden solutiona service katmanı ekleyip Mvc katmanından service katmanına erişim vermemiz gerekir. Service katmanı da data katmanına erişir. data katmanı da core katmanına erişir. böylece MvcUI > Service > Data > Core ile en üstten en alt katmana kadar ulaşılabilmiş olunur.

			base.OnModelCreating(modelBuilder);
		}




	}
}