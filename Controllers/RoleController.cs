using Courses.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Courses.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {

        private readonly RoleManager<IdentityRole> _RoleManager;
        public RoleController(RoleManager<IdentityRole> roleManager ) => _RoleManager = roleManager;
        public async Task<IActionResult> Index()
        {
            return View( await _RoleManager.Roles.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return await Task.FromResult(View());
        }
        [HttpPost]
        public async Task<IActionResult> Add(RoleFormViewModel Role)
        {
            if(ModelState.IsValid)
            await _RoleManager.CreateAsync(new IdentityRole()
            {
                Name = Role.Name.Trim()
            });
            return RedirectToAction(nameof(Index));
        }
    }
}
