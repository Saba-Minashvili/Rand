using RandApp.Models;
using System.Collections.Generic;

namespace RandApp.DTOs
{
    public class ItemDto : BaseEntity
    {
        public string ItemPhoto { get; set; }
        public string ItemType { get; set; }
        public string ItemCategory { get; set; }
        public string Name { get; set; }
        public List<ItemColorsDto> Color { get; set; }
        public List<ItemSizesDto> Size { get; set; }
        public string MaterialType { get; set; }
        public string DesignedFor { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
