using Aura.Core.Entities;

namespace Aura.Core.Models
{
    // Note: doesn't expose events or behavior
    public class UserDto
    {   
        public string Name { get; set; }

        public string Email { get; set; }

        // factory
        public static UserDto FromUser(User user)
        {
            return new UserDto()
            {
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}