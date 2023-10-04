using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    public class CourseSchedule
    {
        [ForeignKey("course")]
        public int CourseID { get; set; }

        [Required ,Column(TypeName ="varchar(45)") ]
        public string Date { get; set; }
        [Required , DataType(DataType.Time)]
        public TimeSpan Start { get; set; }
        [Required ,Range(1,400) ,  DataType(DataType.Time)]
        public TimeSpan End { get; set; }

        public virtual Course Course { get; set; }
    }

    
}
