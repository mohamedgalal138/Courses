using Courses.Models;
using System.ComponentModel.DataAnnotations;

namespace Courses.ViewModels
{
    public class AddCourseViewModel
    {

        [Required(ErrorMessage = "You Can Not Add Course Without Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You Can Not Add Course Without Description")]
        public string Description { get; set; }

        [Required, Range(1,500)]
        public int Capacity { get; set; }


    }
}
