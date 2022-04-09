using RandApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandApp.DTOs
{
    public class CartItemDto : BaseEntity
    {
        public ItemDto Item { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public UserDto User { get; set; }
        public int Quantity { get; set; }
        public string SelectedItemColor { get; set; }
        public string SelectedItemSize { get; set; }
    }
}
