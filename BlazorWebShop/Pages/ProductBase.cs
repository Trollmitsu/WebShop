using BlazorWebShop.Services.Contracts;
using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;

namespace BlazorWebShop.Pages
{
    public class ProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.GetItems();
        }
    }
}
