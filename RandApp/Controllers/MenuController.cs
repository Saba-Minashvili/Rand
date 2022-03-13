using Microsoft.AspNetCore.Mvc;
using RandApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandApp.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult LoadMenuCategory(string designedFor, string category)
        {
            List<int> result = new List<int>();
            if (designedFor != null && category != null)
            {
                if (designedFor.ToLower() == "men" && category.ToLower() == "clothing")
                {
                    var tmp = Enum.GetValues(typeof(Enums.MClothing)).Cast<Enums.MClothing>().Select(o => (int)o).ToList();
                    result = tmp;
                }
                if (designedFor.ToLower() == "men" && category.ToLower() == "bags")
                {
                    var tmp = Enum.GetValues(typeof(Enums.MBags)).Cast<Enums.MBags>().Select(o => (int)o).ToList();
                    result = tmp;
                }
                if (designedFor.ToLower() == "men" && category.ToLower() == "shoes")
                {
                    var tmp = Enum.GetValues(typeof(Enums.MShoes)).Cast<Enums.MShoes>().Select(o => (int)o).ToList();
                    result = tmp;
                }
                if (designedFor.ToLower() == "men" && category.ToLower() == "accessories")
                {
                    var tmp = Enum.GetValues(typeof(Enums.MAccessories)).Cast<Enums.MAccessories>().Select(o => (int)o).ToList();
                    result = tmp;
                }
                if (designedFor.ToLower() == "women" && category.ToLower() == "clothing")
                {
                    var tmp = Enum.GetValues(typeof(Enums.WClothing)).Cast<Enums.WClothing>().Select(o => (int)o).ToList();
                    result = tmp;
                }
                if (designedFor.ToLower() == "women" && category.ToLower() == "bags")
                {
                    var tmp = Enum.GetValues(typeof(Enums.WBags)).Cast<Enums.WBags>().Select(o => (int)o).ToList();
                    result = tmp;
                }
                if (designedFor.ToLower() == "women" && category.ToLower() == "shoes")
                {
                    var tmp = Enum.GetValues(typeof(Enums.WShoes)).Cast<Enums.WShoes>().Select(o => (int)o).ToList();
                    result = tmp;
                }
                if (designedFor.ToLower() == "women" && category.ToLower() == "accessories")
                {
                    var tmp = Enum.GetValues(typeof(Enums.WAccessories)).Cast<Enums.WAccessories>().Select(o => (int)o).ToList();
                    result = tmp;
                }
                if (designedFor.ToLower() == "kids" && category.ToLower() == "clothing")
                {
                    var tmp = Enum.GetValues(typeof(Enums.KClothing)).Cast<Enums.KClothing>().Select(o => (int)o).ToList();
                    result = tmp;
                }
                if (designedFor.ToLower() == "kids" && category.ToLower() == "shoes")
                {
                    var tmp = Enum.GetValues(typeof(Enums.KShoes)).Cast<Enums.KShoes>().Select(o => (int)o).ToList();
                    result = tmp;
                }
                if (designedFor.ToLower() == "kids" && category.ToLower() == "accessories")
                {
                    var tmp = Enum.GetValues(typeof(Enums.KAccessories)).Cast<Enums.KAccessories>().Select(o => (int)o).ToList();
                    result = tmp;
                }
            }
            var menuCategoryViewModel = new MenuCategoryViewModel()
            {
                DesignedFor = designedFor,
                Category = category,
                Types = result
            };
            ViewBag.DesignedFor = designedFor;
            return PartialView("_MenuCategory", menuCategoryViewModel);
        }
    }
}
