using RandApp.Models;
using System.Collections.Generic;

namespace RandApp.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public double CartTotal { get; set; }
    }
}
