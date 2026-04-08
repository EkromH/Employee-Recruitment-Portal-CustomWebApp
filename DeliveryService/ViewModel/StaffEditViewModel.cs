using DeliveryService.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.ViewModel
{
    public class StaffEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the staff name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Display(Name = "Passport Number")]
        public string? PassportNo { get; set; }

        [Display(Name = "Father's Name")]
        public string? FathersName { get; set; }

        [Display(Name = "Mother's Name")]
        public string? MothersName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public StaffStatus Status { get; set; }

        [Display(Name = "Update Photo")]
        public IFormFile? Photo { get; set; } // Nullable because they might not want to change it

        public string? ExistingPhotoPath { get; set; } // Used to display the current image in the View
    }
}
