using DeliveryService.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.ViewModel
{
    public class StaffCreateViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the staff name")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Mobile number is required")]
        [Phone]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; } = null!;

        [Required(ErrorMessage = "Passport number is required")]
        [Display(Name = "Passport No")]
        public string PassportNo { get; set; } = null!;

        [Display(Name = "Father's Name")]
        public string FathersName { get; set; }

        [Display(Name = "Mother's Name")]
        public string MothersName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Profile Photo")]
        public IFormFile? Photo { get; set; } // This handles the actual upload

        [Required]
        public StaffStatus Status { get; set; } = StaffStatus.Selected;
    }
}

