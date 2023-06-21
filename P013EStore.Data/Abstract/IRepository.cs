using System;
using System.Collections.Generic;
using System.Linq.Expressions; // kendi lambda Exğression kullanabileceğimiz metotlar yazmamızı sağlayan kütüphane
using System.Text;
using System.Threading.Tasks;

namespace P013EStore.Data.Abstract
{
	public interface IRepository<T> where T : class // IRepository interface i dışarıdan alacağı T tipinde bir parametreyle çalışacak ve bu t nin veri tipi bir class olmalıdır.
	{
		// Senkron metotlar
		List<T> GetAll(); //db deki
		List<T> GetAll(Expression<Func<T, bool>> expression); // uygulamada verileri listelerken p=>p.IsActive vb gibi sorgulama ve filtreleme kodlarını kullanabilmemizi sağlar.

		T Get(Expression<Func<T, bool>> expression);
		T Find(int id);
		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
		int Save();

		// Asenkron metotlar

		Task<T> FindAsync(int id);
		Task<T> GetAsync(Expression<Func<T, bool>> expression); // lambda expression kullanarak db de filtreleme yapıp geriye 1 tane kayıt döndürür.
		Task<List<T>> GetAllAsync();
		Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression); // lambda expression kullanarak db de filtreleme yapıp geriye liste döndürür.
		Task AddAsync(T entity);
		Task<int> SaveAsync();


	}
}
