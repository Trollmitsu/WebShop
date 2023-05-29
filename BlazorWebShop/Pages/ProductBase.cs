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

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                group product by product.CategoryId
                into prodByVatGroup
                orderby prodByVatGroup.Key
                select prodByVatGroup;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDtos)
        {
            return groupedProductDtos.FirstOrDefault(pg => pg.CategoryId == groupedProductDtos.Key).CategoryName;
        }
    }
}
