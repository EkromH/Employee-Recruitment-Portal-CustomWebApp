using Microsoft.AspNetCore.Identity;

namespace DeliveryService.Models
{
    public class Users : IdentityUser
    {
        public string Name { get; set; }
    }
}
