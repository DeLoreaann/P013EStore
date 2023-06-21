using Microsoft.EntityFrameworkCore;
using P013EStore.Core.Entities;
using P013EStore.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace P013EStore.Data.Concrete
{
    public class ProductRepository : Repository<Product>, IProductRepository // önce ProductRepository'e tıklayıp ampulden generate constractor diycez sonra da IProductRepository üzerine tıklayıp ampulden implement interface diycez.
    {
        public ProductRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<Product> GetProductByIncludeAsync(int id)
        {
            return await _context.Products.Include(p => p.Brand).Include(p=>p.Category).FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<List<Product>> GetProductsByIncludeAsync()
        {
            return await _context.Products.Include(p => p.Brand).Include(p => p.Category).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByIncludeAsync(Expression<Func<Product, bool>> expression)
        {
            return await _context.Products.Where(expression).Include(p => p.Brand).Include(p => p.Category).ToListAsync();
        }
    }
}
