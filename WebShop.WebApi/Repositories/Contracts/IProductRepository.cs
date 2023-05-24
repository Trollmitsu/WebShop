using WebShop.WebApi.Entites;

namespace WebShop.WebApi.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetItems();
        Task<IEnumerable<ProductCategory>> GetCategories();
        Task<Product> GetItem(int Id);
        Task<ProductCategory> GetCategory(int id);

    }
}
