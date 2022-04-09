using System.ComponentModel.DataAnnotations.Schema;

namespace RandApp.Models
{
    public class ItemSizes : BaseEntity
    {
        public string ItemSize { get; set; }
        public int? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
}
