using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace RandApp.Controllers
{
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager = default;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // need to comment "Authorize" attribute in order to use "Role" view and controller
        [Authorize(Policy = "readpolicy")]
        [Route("/Role/Index")]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        // need to comment "Authorize" attribute in order to use "Role" view and controller
        [Authorize(Policy = "writepolicy")]
        [Route("/Role/Create")]
        public IActionResult Create()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }
    }
}
