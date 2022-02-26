using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RandApp.Models
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "FirstName is required")]
        [Display(Name = "First Name")]
        [StringLength(16, ErrorMessage = "Must be between 3 and 16 characters", MinimumLength = 3)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        [Display(Name = "Last Name")]
        [StringLength(16, ErrorMessage = "Must be between 5 and 16 characters", MinimumLength = 5)]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Age")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Please enter valid Number")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        public string AccountId { get; set; }
        public List<CartItem> ShoppingCartList { get; set; } = new List<CartItem>();
    }
}
