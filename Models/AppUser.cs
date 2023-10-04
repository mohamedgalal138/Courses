using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    public class AppUser  :IdentityUser
    {
        [Required ,Column(TypeName ="varchar(45)")]
        public string FullName { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string? ProfilePicture { get; set; }

    }


}
