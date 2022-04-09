using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RandApp.Models
{
    public class Item : BaseEntity
    {
        [Required]
        [Display(Name = "Item Photo")]
        public string ItemPhoto { get; set; }
        [Required]
        [Display(Name = "Item Type")]
        public string ItemType { get; set; }
        [Required]
        [Display(Name = "Item Category")]
        public string ItemCategory { get; set; }
        [Required]
        [Display(Name = "Item Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Item Colors")]
        public List<ItemColors> Color { get; set; } = new List<ItemColors>();
        [Required]
        [Display(Name = "Item Size")]
        public List<ItemSizes> Size { get; set; } = new List<ItemSizes>();
        [Required]
        [Display(Name = "Item Material Type")]
        public string MaterialType { get; set; }
        [Required]
        [Display(Name = "Designed For")]
        public string DesignedFor { get; set; }
        [Required]
        [Display(Name = "Item Price")]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Item Description")]
        public string Description { get; set; }
    }
}
