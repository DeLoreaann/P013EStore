using P013EStore.Core.Entities;

namespace P013EStore.MVCUI.Models
{
    public class ProductDetailViewModel
    {
        public List<Product>? RelatedProducts { get; set; }
        public Product Product { get; set; }

    }
}
