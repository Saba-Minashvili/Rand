﻿using Microsoft.AspNetCore.Mvc;
using RandApp.Models;
using RandApp.Repositories.Abstraction;
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
        [Route("Item/Details")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemRepo.ReadByIdAsync(id);
            return View(item);
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var items = await _itemRepo.ReadAsync();
            return View(items);
        }
    }
}