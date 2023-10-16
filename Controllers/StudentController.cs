using Courses.Data;
using Courses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;

namespace Courses.Controllers
{
    [Authorize(Roles ="Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _Context;
        private readonly UserManager<AppUser> _UserManager;
        public StudentController( ApplicationDbContext context , UserManager<AppUser>  userManager)
        {
            _UserManager = userManager;
            _Context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var Studentid = await GetStudentid();
            var StudentEnrollInCoures = await _Context.StudentCourses.AnyAsync(e=>e.StudentId == Studentid);
            if(!StudentEnrollInCoures)
            {
                return View(await _Context.Courses.Where(e => e.Subscribers < e.Capacity).ToListAsync());
            }
            else
            {
                var StudentCourse = await _Context.StudentCourses.Where(e => e.StudentId == Studentid).Select(e => e.CourseId).ToListAsync();
                var AllCourses = await _Context.Courses.Where(e => e.Subscribers < e.Capacity).ToListAsync();
                var courses =  AllCourses.ExceptBy(StudentCourse, e => e.ID).ToList();
                return View(courses);
            }

        }

        public async Task<IActionResult> Enroll(int id )
        {
            var StudentId = await GetStudentid();
            var Course = await _Context.Courses.FirstOrDefaultAsync(e => e.ID == id);
            if (Course != null && Course.Subscribers < Course.Capacity)
            {
                Course.Subscribers++;
            }

            try
            {
                await _Context.StudentCourses.AddAsync(new StudentCourse()
                {
                    CourseId = id,
                    StudentId = StudentId,
                });
                await _Context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
            
        }

        public async Task<IActionResult> Courses()
        {
           var StudentId = await GetStudentid();
           var Course = await _Context.StudentCourses.Where(e=>e.StudentId == StudentId).Include(e=>e.Course).Select(e=>e.Course).ToListAsync();

            return await Task.FromResult(View(Course));
        }

        public async Task<IActionResult> CancelEnroll(int id)
        {
            var studentid = await GetStudentid();
            var Course = await _Context.Courses.FirstOrDefaultAsync(e => e.ID == id);
            if (Course != null)
            {
                Course.Subscribers--;
            }

            _Context.Entry<StudentCourse>(new StudentCourse()
            {
                CourseId = id,
                StudentId = studentid
            }).State = EntityState.Deleted;
            try
            {
                await _Context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Courses));

        }

        private  async Task<int> GetStudentid()
        {
            var id =  _UserManager.GetUserId(User);
           // string? UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _Context.Students.Where(e => e.AppUserID == id).Select(e => e.Id).SingleOrDefaultAsync();
           
        }
    }
}
