using System.ComponentModel.DataAnnotations;

namespace Courses.ViewModels
{
    public class InstrcutorViewModel
    {
        public string Id { get; set; }

        [Required , MaxLength(100) ]
        public string Name { get; set; }

        [Required , MaxLength(100)  , EmailAddress]
        public string Email { get; set; }
        [Required ]
        public string PassWord { get; set; }

        [Required  , Phone]
        public string PhoneNumber { get; set; }

        [Required  , Range(22 , 59)]
        public int Age { get; set; }



    }
}
