using RandApp.DTOs;
using System.Collections.Generic;

namespace RandApp.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItemDto> CartItems { get; set; }
        public double CartTotal { get; set; }
    }
}
