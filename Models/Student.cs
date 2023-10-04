using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    public class Student 
    {
        public Student()
        {
            StudentCourses = new HashSet<StudentCourse>();
        }
        public int Id { get; set; }
        public DateOnly BirthDate { get; set; }

        public  Gender? Gender { get; set; }

        [ForeignKey("User")]
        public string AppUserID { get; set; }

        public virtual AppUser User { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; set; }

    }

    public enum Gender
    {
        Male,
        female
    }
}
