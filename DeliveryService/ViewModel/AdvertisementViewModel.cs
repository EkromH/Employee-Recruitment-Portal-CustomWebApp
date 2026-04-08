using System.ComponentModel.DataAnnotations;

namespace DeliveryService.ViewModel
{
    public class AdvertisementViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
