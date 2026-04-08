using System.ComponentModel.DataAnnotations;

namespace DeliveryService.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]

        public string Password { get; set; } = null!;
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
