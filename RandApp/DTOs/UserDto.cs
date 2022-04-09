using RandApp.Models;
using System.Collections.Generic;

namespace RandApp.DTOs
{
    public class UserDto : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string AccountId { get; set; }
        public List<CartItemDto> ShoppingCartList { get; set; } = new List<CartItemDto>();
    }
}
