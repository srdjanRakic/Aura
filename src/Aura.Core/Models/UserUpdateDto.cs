using System.ComponentModel.DataAnnotations;

namespace Aura.Core.Models
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "You should provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "You should provide a email value.")]
        public string Email { get; set; }
    }
}
