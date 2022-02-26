using System.ComponentModel.DataAnnotations.Schema;

namespace RandApp.Models
{
    public class CartItem : BaseEntity
    {
        public Item Item { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int Quantity { get; set; }
    }
}
