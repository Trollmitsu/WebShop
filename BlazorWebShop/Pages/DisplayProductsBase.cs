using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;

namespace BlazorWebShop.Pages
{
    public class DisplayProductsBase : ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
