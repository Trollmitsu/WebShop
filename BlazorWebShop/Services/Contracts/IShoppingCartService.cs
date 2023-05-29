using ShopOnline.Models.DTOs;

namespace BlazorWebShop.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<CartItemDto>> GetItems(int userId);
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);

    }
}
