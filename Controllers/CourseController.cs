using Courses.Data;
using Courses.Models;
using Courses.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace Courses.Controllers
{
    [Authorize(Roles ="Instrcutor")]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _Context;
        private readonly UserManager<AppUser> _UserManager;
        public CourseController(ApplicationDbContext context , UserManager<AppUser> userManager)
        {

            _UserManager = userManager;
            _Context = context; 
        }
        
        public async Task<IActionResult> Index()
        {
            var id = await GetInstrcutorId();
            return View(await _Context.Courses.Where(c => c.InstrctorID == id).ToListAsync());
           // return await Task.FromResult(View(await _Context.Courses.Where(c=> c.InstrctorID == id).ToListAsync()));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return await Task.FromResult(View());
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCourseViewModel model , List<string> Days , List<TimeSpan> Starts , List<TimeSpan> Ends,IFormFile image)
        {  
            try
            {
                int i = 0;
                var Schedules = new List<CourseSchedule>();
                while (Days.Count - 1 >= i)
                {
                    Schedules.Add(new CourseSchedule()
                    {
                        Start = Starts[i],
                        End = Ends[i],
                        Date = Days[i]
                    });
                    i++;
                }
                var instrcutorid = await GetInstrcutorId();
                string courseimage = string.Empty;
                if (image is not null)
                {
                    using (FileStream stream = new($".\\wwwroot\\Images\\CoursesImages\\{model.Title}.{image.FileName.Split(".").Last()}", FileMode.Append, FileAccess.Write))
                    {
                        await image.CopyToAsync(stream);
                    }
                    courseimage = $"{model.Title}.{image.FileName.Split('.').Last()}";

                }
                else
                {
                    courseimage = "DefaultImage.png";
                }
                var course = new Course()
                {
                    InstrctorID = instrcutorid,
                    Title = model.Title,
                    Description = model.Description,
                    Capacity = model.Capacity,
                    CoursePicture = courseimage,
                };
                foreach (var item in Schedules)
                {
                    course.CourseSchedules.AddRange(new List<CourseSchedule>()
                {
                     new CourseSchedule()
                     {
                        Date = item.Date,
                        Start = item.Start,
                        End = item.End,
                     }
                });

                }

                await _Context.Courses.AddAsync(course);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detalis(int id)
        {
            ViewBag.CourseTitle = await _Context.Courses.Where(e => e.ID == id).Select(e => e.Title).SingleOrDefaultAsync();
            ViewBag.CourseDesc = await _Context.Courses.Where(e => e.ID == id).Select(e => e.Description).SingleOrDefaultAsync();
            ViewBag.CourseImage = await _Context.Courses.Where(e=>e.ID == id).Select(e=>e.CoursePicture).SingleOrDefaultAsync();
            ViewBag.Students =  await _Context.StudentCourses.Where(e => e.CourseId == id).Include(e => e.Student).ThenInclude(e=>e.User).Select(e=>e.Student).ToListAsync();
            var Schedule = await _Context.CourseSchedule.Where(e => e.CourseID == id).ToListAsync();
            return View(Schedule);

        }

        public async Task<IActionResult> Delete( int id)
        {
            var Course = await GetCourseById(id);
            if(Course is not  null)
            _Context.Courses.Remove(Course);
            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<Course?> GetCourseById(int id)
        {
            var course =  await _Context.Courses.FirstOrDefaultAsync(c => c.ID == id);
            if(course is null)
            {
                return null;
            }
                return course;
        }

        private async Task<int> GetInstrcutorId()
        {
            //  var id = User.Claims.Where(e=>e.Type == ClaimTypes.NameIdentifier).Select(e=>e.Value).FirstOrDefault();
            var id =  _UserManager.GetUserId(User);
            //if (User.Identity.IsAuthenticated)
            //{
            //    foreach (var u in User.Claims)
            //    {
            //        Debug.WriteLine($"type = {u.Type} / value = {u.Value} ");
            //    }
            //}
            // var userid =   User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userid2 = User.Identities.Select(e => e.FindFirst(ClaimTypes.NameIdentifier)).FirstOrDefault();
            return await _Context.Instrctors.Where(e => e.AppUserID == id).Select(e => e.Id).SingleOrDefaultAsync();           
        }
    }
}
