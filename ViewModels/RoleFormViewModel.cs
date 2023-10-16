using System.ComponentModel.DataAnnotations;

namespace Courses.ViewModels
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage ="You Can`t Add Role Without Name") , StringLength(255)]
        public string Name { get; set; }
    }
}
