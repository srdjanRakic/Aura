using System;
using System.ComponentModel.DataAnnotations;
using Aura.Core.SharedKernel;

namespace Aura.Core.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; } = null;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
    }
}