using Microsoft.AspNetCore.Identity;

namespace ChatAppBW.Authentication
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; } = string.Empty;
     
    }
}
