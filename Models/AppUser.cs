using Microsoft.AspNetCore.Identity;

namespace Fitness.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public bool IsActivated { get; set; }
    }
}
