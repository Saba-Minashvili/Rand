using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandApp.Models;
using RandApp.Repositories.Abstraction;
using RandApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository<User> _userRepo = default;
        private readonly IRepository<Item> _itemRepo = default;
        private readonly IRepository<CartItem> _cartItemRepo = default;
        private readonly UserManager<IdentityUser> _userManager = default;
        private readonly SignInManager<IdentityUser> _signManager = default;
        public CartController(IRepository<User> userRepo,
            IRepository<Item> itemRepo,
            UserManager<IdentityUser> userManager,
            IRepository<CartItem> cartItemRepo,
            SignInManager<IdentityUser> signManager)
        {
            _userRepo = userRepo;
            _itemRepo = itemRepo;
            _cartItemRepo = cartItemRepo;
            _userManager = userManager;
            _signManager = signManager;
        }

        [Route("/cart/index")]
        public IActionResult Index()
        {
            double total = 0;
            var cartItems = _cartItemRepo.Get().Where(o => o.UserId == GetCurrentUserId()).Include(o => o.Item).ToListAsync().Result;
            var filteredIds = cartItems.Select(o => o.Item.Id).Distinct().ToList();
            var filteredCartItem = new CartItem();
            var result = new List<CartItem>();
            foreach (var id in filteredIds)
            {
                filteredCartItem = cartItems.FirstOrDefault(o => o.Item.Id == id);
                result.Add(filteredCartItem);
                total += (filteredCartItem.Item.Price * filteredCartItem.Quantity);
            }
            var shoppingCartViewModel = new ShoppingCartViewModel()
            {
                CartItems = result,
                CartTotal = total
            };
            return View(shoppingCartViewModel);
        }

        public async Task<IActionResult> Add(int itemId)
        {
            var currentUser = await _userRepo.ReadByIdAsync(GetCurrentUserId());
            if (currentUser == null)
            {
                return NotFound();
            }
            var item = await _itemRepo.ReadByIdAsync(itemId);
            if (item == null)
            {
                return NotFound();
            }
            var cartItem = new CartItem() { Item = item, User = currentUser, UserId = currentUser.Id, Quantity = 1 };
            currentUser.ShoppingCartList.Add(cartItem);
            var cartItems = _cartItemRepo.Get().Where(o => o.UserId == GetCurrentUserId()).Include(o => o.Item).ToListAsync().Result;
            if (cartItems.Count != 0)
            {
                foreach (var obj in cartItems)
                {
                    if (obj.Item.Id == itemId)
                    {
                        obj.Quantity += 1;
                    }
                }

                await _userRepo.UpdateAsync(currentUser);
                return RedirectToAction("Index");
            }
            else
            {
                await _userRepo.UpdateAsync(currentUser);
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cartItem = await _cartItemRepo.ReadByIdAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            await _cartItemRepo.DeleteAsync(cartItem);
            return RedirectToAction("Index");
        }

        public int GetCurrentUserId()
        {
            int currentUserId = 0;
            if (_signManager.IsSignedIn(User))
            {
                // getting account Id which is a foreign key for user
                string accountId = _userManager.GetUserId(User);
                // getting and loop all users to find match
                var allUsers = _userRepo.ReadAsync();
                foreach (var user in allUsers.Result)
                {
                    if (user.AccountId == accountId)
                    {
                        currentUserId = user.Id;
                        return currentUserId;
                    }
                }
            }

            return currentUserId;
        }
    }
}
