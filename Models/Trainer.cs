using System.ComponentModel.DataAnnotations;

namespace Fitness.Models
{
    public class Trainer:BaseModel
    {
        [Required, MaxLength(20)]
        public string Name { get; set; }
        public string ImageURL { get; set; }
        [Required, Length(5,50)]
        public string Description { get; set; }
        [Required, MinLength(4)]
        public string Speciality { get; set; }
    }
}
