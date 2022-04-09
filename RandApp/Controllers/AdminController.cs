using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandApp.DTOs;
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
        private readonly IRepository<ItemColors> _itemColorsRepo = default;
        private readonly IRepository<ItemSizes> _itemSizesRepo = default;
        private readonly IRepository<CartItem> _cartItemRepo = default;
        private readonly IMapper _mapper = default;
        private IWebHostEnvironment _webHostEnvironment;

        public AdminController(IRepository<Item> itemRepo,
            IRepository<User> userRepo,
            IRepository<CartItem> cartItemRepo,
            IRepository<ItemColors> itemColorsRepo,
            IRepository<ItemSizes> itemSizesRepo,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
        {
            _itemRepo = itemRepo;
            _userRepo = userRepo;
            _cartItemRepo = cartItemRepo;
            _itemColorsRepo = itemColorsRepo;
            _itemSizesRepo = itemSizesRepo;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        [Route("/admin")]
        public async Task<IActionResult> Index()
        {
            var items = await _itemRepo.Get().Include(o => o.Color).Include(o => o.Size).ToListAsync();
            var result = _mapper.Map<IEnumerable<ItemDto>>(items);
            return View(result);
        }


        public IActionResult CreateItem()
        {
            return View();
        }

        // POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(Item item, IFormFile ItemPhoto, List<string> Color, List<string> Size)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", ItemPhoto.FileName);
            var stream = new FileStream(path, FileMode.Create);
            await ItemPhoto.CopyToAsync(stream);
            item.ItemPhoto = ItemPhoto.FileName;

            foreach (var color in Color)
            {
                item.Color.Add(new ItemColors() { ItemId = item.Id, ItemColor = color });
            }
            foreach (var size in Size)
            {
                item.Size.Add(new ItemSizes() { ItemId = item.Id, ItemSize = size });
            }

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

            var result = _mapper.Map<ItemDto>(item);
            return View(result);
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

            var users = _userRepo.ReadAsync().Result.ToList();

            if (DeleteItemFromCart(users, item).Result && DeleteRelatedToItem(item).Result && DeletePhotosFromAssets(item))
            {
                await _itemRepo.DeleteAsync(item);
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
            var item = await _itemRepo.Get().Include(o => o.Color).Include(o => o.Size).FirstOrDefaultAsync(o => o.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<ItemDto>(item);
            return View(result);
        }

        // POST Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateItem(Item item, List<string> Color, List<string> Size)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // this was needed because when i try to get item from "UpdateItem View, it doesn't include
            // list parameters of "Color" and "Size" classes
            // it can be said that it is some kind of overload of "Item" object
            // that was sent from front via form
            item = await _itemRepo.Get().Include(o => o.Color).Include(o => o.Size).FirstOrDefaultAsync(o => o.Id == item.Id);

            foreach (var color in Color)
            {
                if (item.Color.Where(o => o.ItemColor == color).Count() == 0)
                {
                    item.Color.Add(new ItemColors() { ItemId = item.Id, ItemColor = color });
                }
                else
                {
                    continue;
                }
            }
            foreach (var size in Size)
            {
                if (item.Size.Where(o => o.ItemSize == size).Count() == 0)
                {
                    item.Size.Add(new ItemSizes() { ItemId = item.Id, ItemSize = size });
                }
                else
                {
                    continue;
                }
            }

            await _itemRepo.UpdateAsync(item);
            return RedirectToAction("Index");
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

        [HttpGet]
        public List<int> LoadItemSizes(string designedFor, string category, string itemType)
        {
            var result = new List<int>();
            if (designedFor != "" && category != "" && itemType != "")
            {
                switch (designedFor.ToLower(), category.ToLower(), itemType.ToLower())
                {
                    case ("men", "accessories", "belts"):
                    case ("women", "accessories", "belts"):
                        result = Enum.GetValues(typeof(Enums.BeltSize)).Cast<Enums.BeltSize>().Select(o => (int)o).ToList();
                        break;
                    case ("men", "accessories", "ties"):
                        result = Enum.GetValues(typeof(Enums.TieSize)).Cast<Enums.TieSize>().Select(o => (int)o).ToList();
                        break;
                    default:
                        result = Enum.GetValues(typeof(Enums.SingleSize)).Cast<Enums.SingleSize>().Select(o => (int)o).ToList();
                        break;
                }
            }

            return result;
        }


        // this method is for item categories that have same sizes and
        // don't require specifying item type
        [HttpGet]
        public List<string> LoadItemSize(string designedFor, string category)
        {
            var result = new List<string>();
            var enumValues = new List<int>();
            if (designedFor != "" && category != "")
            {
                switch (designedFor.ToLower(), category.ToLower())
                {
                    case ("kids", "shoes"):
                        result = Enum.GetNames(typeof(Enums.KShoes)).ToList();
                        break;
                    case ("men", "shoes"):
                    case ("women", "shoes"):
                        enumValues = Enum.GetValues(typeof(Enums.ShoeSize)).Cast<Enums.ShoeSize>().Select(o => (int)o).ToList();
                        foreach (var item in enumValues)
                        {
                            result.Add(item.ToString());
                        }
                        break;
                    case ("men", "clothing"):
                    case ("women", "clothing"):
                    case ("kids", "clothing"):
                        result = Enum.GetNames(typeof(Enums.ClothingSize)).ToList();
                        break;
                    case ("men", "accessories"):
                    case ("women", "accessories"):
                    case ("kids", "accessories"):
                        result = Enum.GetNames(typeof(Enums.SingleSize)).ToList();
                        break;
                    case ("men", "hats"):
                    case ("women", "hats"):
                    case ("kids", "hats"):
                        result = Enum.GetNames(typeof(Enums.SingleSize)).ToList();
                        break;
                }
            }

            return result;
        }

        public async Task<bool> DeleteItemFromCart(List<User> users, Item item)
        {
            var cartItems = new List<CartItem>();

            if (cartItems.Count > 0)
            {
                foreach (var user in users)
                {
                    cartItems = _cartItemRepo.Get().Where(o => o.UserId == user.Id).Include(o => o.Item).ToListAsync().Result;
                    var cartItem = cartItems.FirstOrDefault(o => o.Item.Id == item.Id);
                    if (cartItem != null)
                    {
                        await _cartItemRepo.DeleteAsync(cartItem);
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteRelatedToItem(Item item)
        {
            // removing related itemColor objects which is connected to an item with FK
            var itemColors = _itemColorsRepo.Get().Where(o => o.ItemId == item.Id).ToListAsync().Result;
            // removing related itemSize objects which is connected to an item with FK
            var itemSizes = _itemSizesRepo.Get().Where(o => o.ItemId == item.Id).ToListAsync().Result;
            if (itemColors.Count > 0 || itemSizes.Count > 0)
            {
                foreach (var color in itemColors)
                {
                    await _itemColorsRepo.DeleteAsync(color);
                }
                foreach (var size in itemSizes)
                {
                    await _itemSizesRepo.DeleteAsync(size);
                }

                return true;
            }

            return false;
        }

        public bool DeletePhotosFromAssets(Item item)
        {
            // This code is needed to delete the photos of items
            // which are located in wwwroot/assets folder
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", item.ItemPhoto);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
                return true;
            }

            return false;
        }
    }
}
