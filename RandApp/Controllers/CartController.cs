using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandApp.DTOs;
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
        private readonly IMapper _mapper = default;
        public CartController(IRepository<User> userRepo,
            IRepository<Item> itemRepo,
            UserManager<IdentityUser> userManager,
            IRepository<CartItem> cartItemRepo,
            SignInManager<IdentityUser> signManager,
            IMapper mapper)
        {
            _userRepo = userRepo;
            _itemRepo = itemRepo;
            _cartItemRepo = cartItemRepo;
            _userManager = userManager;
            _signManager = signManager;
            _mapper = mapper;
        }

        [Route("/cart/index")]
        public async Task<IActionResult> Index()
        {
            double total = 0;
            // getting all cart items of current user
            var cartItems = await _cartItemRepo.Get().Where(o => o.UserId == GetCurrentUserId()).Include(o => o.Item).ToListAsync();
            // making sure to have distinct ids to filter easyly
            var filteredIds = cartItems.Select(o => o.Item.Id).Distinct().ToList();

            // defining variables to save filtered results in them
            var filteredCartItem = new CartItem();
            var filteredCartItems = new List<CartItem>();

            foreach (var id in filteredIds)
            {
                var tmp = cartItems.Where(o => o.Item.Id == id).ToList();
                foreach (var carItem in tmp)
                {

                    filteredCartItems.Add(carItem);
                }
                total += cartItems.FirstOrDefault(o => o.Item.Id == id).Item.Price *
                          cartItems.FirstOrDefault(o => o.Item.Id == id).Quantity;
            }

            var result = _mapper.Map<List<CartItemDto>>(filteredCartItems);
            var shoppingCartViewModel = new ShoppingCartViewModel()
            {
                CartItems = result,
                CartTotal = total
            };

            return View(shoppingCartViewModel);
        }

        public async Task<bool> Add(int itemId, string itemColor, string itemSize)
        {
            var currentUser = await _userRepo.ReadByIdAsync(GetCurrentUserId());
            if (currentUser == null)
            {
                return false;
            }

            var item = await _itemRepo.Get().Include(o => o.Color).Include(o => o.Size).FirstOrDefaultAsync(o => o.Id == itemId);
            if (item == null)
            {
                return false;
            }

            var cartItem = new CartItem()
            {
                Item = item,
                User = currentUser,
                UserId = currentUser.Id,
                Quantity = 1,
                SelectedItemColor = itemColor,
                SelectedItemSize = itemSize
            };

            var cartItems = _cartItemRepo.Get().Where(o => o.UserId == GetCurrentUserId()).Include(o => o.Item).ToListAsync().Result;
            if (cartItems.Count > 0)
            {
                foreach (var obj in cartItems)
                {
                    if (obj.SelectedItemColor == cartItem.SelectedItemColor && obj.SelectedItemSize == cartItem.SelectedItemSize)
                    {
                        currentUser.ShoppingCartList.Remove(obj);
                        obj.Quantity += 1;
                        currentUser.ShoppingCartList.Add(obj);
                        await _userRepo.UpdateAsync(currentUser);
                        return true;
                    }
                }

                currentUser.ShoppingCartList.Add(cartItem);
                await _userRepo.UpdateAsync(currentUser);
                return true;
            }
            else
            {
                currentUser.ShoppingCartList.Add(cartItem);
            }

            await _userRepo.UpdateAsync(currentUser);
            return true;
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
