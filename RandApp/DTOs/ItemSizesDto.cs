using RandApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandApp.DTOs
{
    public class ItemSizesDto : BaseEntity
    {
        public string ItemSize { get; set; }
        public int? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public ItemDto Item { get; set; }
    }
}
