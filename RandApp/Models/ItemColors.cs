using System.ComponentModel.DataAnnotations.Schema;

namespace RandApp.Models
{
    public class ItemColors : BaseEntity
    {
        public string ItemColor { get; set; }
        public int? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
}
