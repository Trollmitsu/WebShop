﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders.Testing;
using ShopOnline.Models.DTOs;
using WebShop.WebApi.Data;
using WebShop.WebApi.Entites;
using WebShop.WebApi.Repositories.Contracts;

namespace WebShop.WebApi.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await this.shopOnlineDbContext.CartItems.AnyAsync(c => c.CartId == cartId &&
                                                                     c.ProductId == productId);

        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in this.shopOnlineDbContext.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = product.Id,
                                      Quantity = cartItemToAddDto.Quantity
                                  }).SingleOrDefaultAsync();

                if (item != null)
                {
                    var result = await this.shopOnlineDbContext.CartItems.AddAsync(item);
                    await this.shopOnlineDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;

        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await this.shopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                this.shopOnlineDbContext.CartItems.Remove(item);
                await this.shopOnlineDbContext.SaveChangesAsync();
            }

            return item;

        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in this.shopOnlineDbContext.Carts
                          join cartItem in this.shopOnlineDbContext.CartItems
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
            return await (from cart in this.shopOnlineDbContext.Carts
                          join cartItem in this.shopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Quantity = cartItem.Quantity,
                              CartId = cartItem.CartId
                          }).ToListAsync();
        }

        public async Task<CartItem> UpdateQuantity(int id, CartItemQuantityUpdateDto cartItemQuantityUpdateDto)
        {
            var item = await this.shopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Quantity = cartItemQuantityUpdateDto.Quantity;
                await this.shopOnlineDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }
    }
}
