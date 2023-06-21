using P013EStore.Data;
using P013EStore.Data.Concrete;
using P013EStore.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P013EStore.Service.Concrete
{
    public class ProductService : ProductRepository, IProductService
    {
        public ProductService(DatabaseContext context) : base(context)
        {

        }
    }
}
