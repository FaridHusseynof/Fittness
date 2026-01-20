using System.ComponentModel.DataAnnotations;

namespace Fitness.Areas.AdminPanel.ViewModels
{
    public class CreateVM
    {
        [Required, MaxLength(20)]
        public string name { get; set; }
        public string? imageURL { get; set; }
        [Required, Length(5, 50)]
        public string description { get; set; }
        [Required, MinLength(4)]
        public string speciality { get; set; }
        public IFormFile? imageFile { get; set; }
    }
}
