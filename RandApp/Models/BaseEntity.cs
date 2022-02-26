using System.ComponentModel.DataAnnotations;

namespace RandApp.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
