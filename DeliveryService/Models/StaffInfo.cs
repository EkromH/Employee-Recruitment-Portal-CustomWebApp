using DeliveryService.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Models
{
    public class StaffInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required]
        [Phone]
        public string? MobileNumber { get; set; } 

        [Required]
        public string? PassportNo { get; set; } 

        public string? FathersName { get; set; } 

        public string? MothersName { get; set; } 

        public DateTime Date { get; set; }

        public string Photo { get; set; } = null!;
        public StaffStatus Status { get; set; }=StaffStatus.Selected;

    }
}
