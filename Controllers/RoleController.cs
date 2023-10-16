using Courses.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            if (!ModelState.IsValid)
                return View("index");
          
            if (await _RoleManager.RoleExistsAsync(Role.Name))
            {
                ModelState.AddModelError("Name", "This Role Is Exist");
                return View("index", await _RoleManager.Roles.ToListAsync());
            }
            await _RoleManager.CreateAsync(new IdentityRole()
            {
                Name = Role.Name.Trim()
            });

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role =  await _RoleManager.FindByIdAsync(id);
            await _RoleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }
    }
}
