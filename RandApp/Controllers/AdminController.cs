using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandApp.Models;
using RandApp.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RandApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRepository<Item> _itemRepo = default;
        private readonly IRepository<User> _userRepo = default;
        private readonly IRepository<CartItem> _cartItemRepo = default;
        private IWebHostEnvironment _webHostEnvironment;

        public AdminController(IRepository<Item> itemRepo, IRepository<User> userRepo, IRepository<CartItem> cartItemRepo, IWebHostEnvironment webHostEnvironment)
        {
            _itemRepo = itemRepo;
            _userRepo = userRepo;
            _cartItemRepo = cartItemRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/admin")]
        public async Task<IActionResult> Index()
        {
            var items = await _itemRepo.ReadAsync();
            return View(items);
        }


        public IActionResult CreateItem()
        {
            return View();
        }

        // POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(Item item, IFormFile ItemPhoto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", ItemPhoto.FileName);
            var stream = new FileStream(path, FileMode.Create);
            await ItemPhoto.CopyToAsync(stream);
            item.ItemPhoto = ItemPhoto.FileName;

            await _itemRepo.CreateAsync(item);
            stream.Close();
            return RedirectToAction("Index");
        }

        // GET Delete
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var item = await _itemRepo.ReadByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItemPost(int id)
        {
            var item = await _itemRepo.ReadByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            // My goal is to get all users and their shopping cart lists
            // in order to search for items which are added in them
            // So i could delete them from cart when i delete this particular items
            // from admin page
            // It is the worst practice to use
            // and i am still working on it
            var users = _userRepo.ReadAsync().Result.ToList();
            var cartItems = new List<CartItem>();
            foreach (var user in users)
            {
                cartItems = _cartItemRepo.Get().Where(o => o.UserId == user.Id).Include(o => o.Item).ToListAsync().Result;
                var result = cartItems.FirstOrDefault(o => o.Item.Id == item.Id);
                if (result != null)
                {
                    await _cartItemRepo.DeleteAsync(result);
                }
            }



            if (await _itemRepo.DeleteAsync(item))
            {
                // This code is needed to delete the photos of items
                // which are located in wwwroot/assets folder
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", item.ItemPhoto);
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET Update
        public async Task<IActionResult> UpdateItem(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var item = await _itemRepo.ReadByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateItem(Item item)
        {
            if (ModelState.IsValid)
            {
                await _itemRepo.UpdateAsync(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpGet]
        public List<string> LoadItemCategories(string designedFor)
        {
            var result = new List<string>();

            if (designedFor.ToLower() == "men")
            {
                var tmp = Enum.GetNames(typeof(Enums.Category)).ToList();
                result = tmp;
            }
            else if (designedFor.ToLower() == "women")
            {
                var tmp = Enum.GetNames(typeof(Enums.Category)).ToList();
                result = tmp;
            }
            else if (designedFor.ToLower() == "kids")
            {
                var tmp = Enum.GetNames(typeof(Enums.Category)).Where(o => o.ToString() != Enums.Category.Bags.ToString()).ToList();
                result = tmp;
            }

            return result;
        }

        [HttpGet]
        public List<string> LoadItemTypes(string designedFor, string category)
        {
            var result = new List<string>();

            if (designedFor != "" && category != "")
            {
                switch ((designedFor.ToLower(), category.ToLower()))
                {
                    case ("men", "clothing"):
                        result = Enum.GetNames(typeof(Enums.MClothing)).ToList();
                        break;
                    case ("men", "bags"):
                        result = Enum.GetNames(typeof(Enums.MBags)).ToList();
                        break;
                    case ("men", "shoes"):
                        result = Enum.GetNames(typeof(Enums.MShoes)).ToList();
                        break;
                    case ("men", "accessories"):
                        result = Enum.GetNames(typeof(Enums.MAccessories)).ToList();
                        break;
                    case ("women", "clothing"):
                        result = Enum.GetNames(typeof(Enums.WClothing)).ToList();
                        break;
                    case ("women", "bags"):
                        result = Enum.GetNames(typeof(Enums.WBags)).ToList();
                        break;
                    case ("women", "shoes"):
                        result = Enum.GetNames(typeof(Enums.WShoes)).ToList();
                        break;
                    case ("women", "accessories"):
                        result = Enum.GetNames(typeof(Enums.WAccessories)).ToList();
                        break;
                    case ("kids", "clothing"):
                        result = Enum.GetNames(typeof(Enums.KClothing)).ToList();
                        break;
                    case ("kids", "shoes"):
                        result = Enum.GetNames(typeof(Enums.KShoes)).ToList();
                        break;
                    case ("kids", "accessories"):
                        result = Enum.GetNames(typeof(Enums.KAccessories)).ToList();
                        break;
                }
            }

            return result;
        }
    }
}
