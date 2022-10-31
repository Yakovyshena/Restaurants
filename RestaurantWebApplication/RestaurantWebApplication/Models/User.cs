using Microsoft.AspNetCore.Identity;

namespace RestaurantWebApplication.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
