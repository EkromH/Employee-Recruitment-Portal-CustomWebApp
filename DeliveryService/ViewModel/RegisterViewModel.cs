using System.ComponentModel.DataAnnotations;

namespace DeliveryService.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Please Enter Correct Email Address")]
        [EmailAddress]

        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Please Enter Password")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} character long")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Paassword does not match")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "retype pass")]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; } = null!;
    }
}
