using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    public class Instrctor 
    {
        public int Id { get; set; }
        public int Age { get; set; }

        [ForeignKey("User")]
        public string AppUserID { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

    }
}
