using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandApp.DTOs;
using RandApp.Models;
using RandApp.Repositories.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly IRepository<Item> _itemRepo = default;
        private readonly IMapper _mapper = default;

        public ItemController(IRepository<Item> itemRepo, IMapper mapper)
        {
            _itemRepo = itemRepo;
            _mapper = mapper;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var items = await _itemRepo.ReadAsync();
            var result = _mapper.Map<IEnumerable<ItemDto>>(items);
            return View(result);
        }

        [Route("/items/{designedFor}")]
        public IActionResult IndexFor(string designedFor)
        {
            var items = _itemRepo.Get().Include(o => o.Color).Include(o => o.Size).ToList();
            var filteredItems = items.FindAll(o => o.DesignedFor.ToLower() == designedFor.ToLower());
            ViewBag.DesignedFor = designedFor.ToLower();
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            var result = _mapper.Map<List<ItemDto>>(filteredItems);
            return View(result);
        }

        [Route("/items/{designedFor}/{category}/{type}")]
        public IActionResult IndexFor(string designedFor, string category, string type)
        {
            if (ModelState.IsValid)
            {
                var items = _itemRepo.Get().Include(o => o.Color).Include(o => o.Size).ToList();
                var filteredItems = items
                    .FindAll(o => o.DesignedFor.ToLower() == designedFor.ToLower())
                    .Where(o => o.ItemCategory.ToLower() == category.ToLower() && o.ItemType.ToLower() == type.ToLower())
                    .ToList();
                ViewBag.returnUrl = Request.Headers["Referer"].ToString();
                var result = _mapper.Map<List<ItemDto>>(filteredItems);
                return View(result);
            }

            return View();
        }

        [HttpGet]
        [Route("item/details")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemRepo.Get().Include(o => o.Color).Include(o => o.Size).FirstOrDefaultAsync(o => o.Id == id);
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            var result = _mapper.Map<ItemDto>(item);
            return View(result);
        }


        public IActionResult ReturnUrl(string returnUrl)
        {
            return Redirect(returnUrl);
        }
    }
}
