using BlazorWebShop.Services.Contracts;
using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;

namespace BlazorWebShop.Pages
{
    public class ShoppingCartBase:ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }

        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CalculateCartSummaryTotals();
            }
            catch (Exception ex)
            {
               ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);

            RemoveCartItem(id);

            CalculateCartSummaryTotals();

        }

        protected async Task UpdateQuantityItem_Click(int id, int quantity)
        {
            try
            {
                if (quantity > 0)
                {
                    var updateItemDto = new CartItemQuantityUpdateDto
                    {
                        CartItemId = id,
                        Quantity = quantity,
                    };
                    var returnedUpdateItemDto = await ShoppingCartService.UpdateQuantity(updateItemDto);

                    UpdateItemTotalPrice(returnedUpdateItemDto);

                    CalculateCartSummaryTotals();
                }
                else
                {
                    var item = ShoppingCartItems.FirstOrDefault(x => x.Id == id);
                    if (item != null)
                    {
                        item.Quantity = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {
               
                throw;
            }
        }

        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Quantity;
            }
        }

        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(p => p.Quantity);
        }
        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
        }
        private void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);

            ShoppingCartItems.Remove(cartItemDto);

        }
    }
}
