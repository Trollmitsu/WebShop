using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders.Testing;
using ShopOnline.Models.DTOs;
using WebShop.WebApi.Data;
using WebShop.WebApi.Entites;
using WebShop.WebApi.Repositories.Contracts;

namespace WebShop.WebApi.Repositories
{
    public class ShoppingCartRepository:IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext _shopOnlineDbContext;

        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            _shopOnlineDbContext = shopOnlineDbContext;
        }

        private async Task<bool> CartItemExists(int cartId, int ProductId)
        {
            return await _shopOnlineDbContext.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == ProductId);
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in _shopOnlineDbContext.Products
                    where product.Id == cartItemToAddDto.ProductId
                    select new CartItem
                    {
                        CartId = cartItemToAddDto.CartId,
                        ProductId = product.Id,
                        Quantity = cartItemToAddDto.Quantity,
                    }).SingleOrDefaultAsync();
                if (item != null)
                {
                    var result = await _shopOnlineDbContext.CartItems.AddAsync(item);
                    await _shopOnlineDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }


            return null;
        }

        public async Task<CartItem> UpdateQuantity(int id, CartItemQuantityUpdateDto cartItemQuantityUpdateDto)
        {
            var item = await _shopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Quantity = cartItemQuantityUpdateDto.Quantity;
                await _shopOnlineDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await _shopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                _shopOnlineDbContext.CartItems.Remove(item);
                await _shopOnlineDbContext.SaveChangesAsync();
            }

            return item;

        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in _shopOnlineDbContext.Carts
                join cartItem in _shopOnlineDbContext.CartItems
                    on cart.Id equals cartItem.CartId
                where cartItem.Id == id
                select new CartItem
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    CartId = cartItem.CartId
                }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in _shopOnlineDbContext.Carts
                join cartItem in _shopOnlineDbContext.CartItems
                    on cart.Id equals cartItem.CartId
                where cart.UserId == userId
                select new CartItem
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    CartId = cartItem.CartId,
                }).ToListAsync();
        }

    }
}
