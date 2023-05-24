using ShopOnline.Models.DTOs;

namespace BlazorWebShop.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetItems();
    }
}
