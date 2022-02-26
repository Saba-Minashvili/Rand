using Microsoft.AspNetCore.Mvc;
using RandApp.Models;
using RandApp.Repositories.Abstraction;
using System.Threading.Tasks;

namespace RandApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Item> _itemRepo = default;

        public HomeController(IRepository<Item> itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _itemRepo.ReadAsync();
            return View(items);
        }
    }
}
