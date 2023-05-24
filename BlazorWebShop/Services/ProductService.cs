using System.Net.Http.Json;
using BlazorWebShop.Services.Contracts;
using ShopOnline.Models.DTOs;

namespace BlazorWebShop.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/Product");
                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
