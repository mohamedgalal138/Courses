using Courses.Data;
using Courses.Models;
using Courses.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Courses.Controllers
{
    [Authorize(Roles ="Admin")]
    public class InstrcutorController : Controller
    {
        //private readonly IUserStore<AppUser> _UserStore;
        private readonly UserManager<AppUser> _UserManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _Context;
        public InstrcutorController(UserManager<AppUser> userManager , ApplicationDbContext context, RoleManager<IdentityRole> roleManager /*IUserStore<AppUser> userStore*/)
        {
            _UserManager = userManager;
            _Context = context;
            _roleManager = roleManager;
            //_UserStore = userStore;
        }
        public async Task<IActionResult> Index()
        {
            var instrcutors = await _Context.Instrctors.Select(  e => new InstrcutorViewModel()
            {
              Name=e.User.FullName,
              Email = e.User.Email??"Null",
              PhoneNumber = e.User.PhoneNumber??"Not Found",
              Age = e.Age
            }).ToListAsync();
            return View(instrcutors);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {

            return await Task.FromResult(View());
        }
        [HttpPost]
        public async Task<IActionResult> Add( AppUser user , string PassWord , int Age)
        {
            //var userId = _Context.Users.Cl.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            //var user1 = await _Context.UserClaims.FirstOrDefaultAsync(u=>u.ClaimType == ClaimTypes.NameIdentifier);
            await _UserManager.SetUserNameAsync(user, user.Email);
            await _UserManager.SetEmailAsync(user, user.Email);
            await _UserManager.CreateAsync(user, PassWord);
            
           // await _UserStore.SetUserNameAsync(user , user.Email , CancellationToken.None);
           if(!await _roleManager.RoleExistsAsync("Instrcutor"))
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Instrcutor"
                });

            }
            await _UserManager.AddToRoleAsync( user, "Instrcutor");
           
            var userid = await _UserManager.GetUserIdAsync(user);
            await _Context.Instrctors.AddAsync(new Instrctor()
            {
                AppUserID = user.Id,
                Age = Age,
                User = user 
            });
            await _Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
