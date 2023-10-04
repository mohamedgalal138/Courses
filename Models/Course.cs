using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    public class Course
    {
        public Course()
        {
            StudentCourses = new HashSet<StudentCourse>();
            CourseSchedules = new List<CourseSchedule>();

        }
        [Key]
        public int ID { get; set; }
        [Required , Column(TypeName ="varchar(100)"), MinLength(5), MaxLength(100)]
        public string Title { get; set; }

        [Required, Column(TypeName = "varchar(400)") , MinLength(50), MaxLength(400)]
        public string Description { get; set; }

        [Range(1,500 )]
        public int Capacity { get; set; }
        [NotMapped]
        public WeekDays Days { get; set; }
        
        public int Subscribers {  get; set; }
        [Column(TypeName ="varchar(200)")]
        public string? CoursePicture { get; set; }

        [ForeignKey("Instrctor")]
        public int InstrctorID { get; set; }

        public virtual Instrctor Instrctor { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; set; }

        public virtual ICollection<CourseSchedule>  CourseSchedules { get; set; }

    }

    public enum WeekDays
    {
         Saturday,
         Sunday,
         Monday,
         Tuesday,
         Wednesday,
         Thursday,
         Friday, 
    }
}
