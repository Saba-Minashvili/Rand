using Microsoft.AspNetCore.Mvc;
using RandApp.Models;
using RandApp.Repositories.Abstraction;
using System.Linq;
using System.Threading.Tasks;

namespace RandApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly IRepository<Item> _itemRepo = default;

        public ItemController(IRepository<Item> itemRepo)
        {
            _itemRepo = itemRepo;
        }

        [HttpGet]
        [Route("item/details")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemRepo.ReadByIdAsync(id);
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            return View(item);
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var items = await _itemRepo.ReadAsync();
            return View(items);
        }

        [Route("/items/{designedFor}")]
        public IActionResult IndexFor(string designedFor)
        {
            var items = _itemRepo.ReadAsync().Result.ToList();
            var result = items.FindAll(o => o.DesignedFor.ToLower() == designedFor.ToLower());
            ViewBag.DesignedFor = designedFor.ToLower();
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            return View(result);
        }

        [Route("/items/{designedFor}/{category}/{type}")]
        public IActionResult IndexFor(string designedFor, string category, string type)
        {
            if (ModelState.IsValid)
            {
                var items = _itemRepo.ReadAsync().Result.ToList();
                var result = items
                    .FindAll(o => o.DesignedFor.ToLower() == designedFor.ToLower())
                    .Where(o => o.ItemCategory.ToLower() == category.ToLower() && o.ItemType.ToLower() == type.ToLower())
                    .ToList();
                ViewBag.returnUrl = Request.Headers["Referer"].ToString();
                return View(result);
            }

            return View();
        }

        public IActionResult ReturnUrl(string returnUrl)
        {
            return Redirect(returnUrl);
        }
    }
}
