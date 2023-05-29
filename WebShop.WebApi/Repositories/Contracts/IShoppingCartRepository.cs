using ShopOnline.Models.DTOs;
using WebShop.WebApi.Entites;

namespace WebShop.WebApi.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItem> UpdateQuantity(int id, CartItemQuantityUpdateDto cartItemQuantityUpdateDto);
        Task <CartItem> DeleteItem(int id);
        Task<CartItem> GetItem(int id);
        Task<IEnumerable<CartItem>> GetItems(int userId);

    }
}
