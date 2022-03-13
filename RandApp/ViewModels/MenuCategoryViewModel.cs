using System.Collections.Generic;

namespace RandApp.ViewModels
{
    public class MenuCategoryViewModel
    {
        public string DesignedFor { get; set; }
        public string Category { get; set; }
        public List<int> Types { get; set; } = new List<int>();
    }
}
