using Microsoft.EntityFrameworkCore;
using WebShop.WebApi.Data;
using WebShop.WebApi.Entites;
using WebShop.WebApi.Repositories.Contracts;

namespace WebShop.WebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _shopOnlineDbContext;

        public ProductRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            _shopOnlineDbContext = shopOnlineDbContext;
        }
        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await _shopOnlineDbContext.Products.ToListAsync();
            return products;
        }

        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await _shopOnlineDbContext.ProductCategories.ToListAsync();
            return categories;
        }

        public async Task<Product> GetItem(int Id)
        {
            var product = await _shopOnlineDbContext.Products.FindAsync(Id);
            return product;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await _shopOnlineDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
            return category;
        }
    }
}
