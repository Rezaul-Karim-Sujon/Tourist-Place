using System.ComponentModel.DataAnnotations;

namespace Tourist_Place.Models.Entity
{
    public class Place
    {
        //[Required]
        //public int PlaceID { get; set; }

        [Required]
        [Key]
        [Display(Name = "Name")]
        public string PlaceName { get; set; }

        [Required]
        public string Address { get; set; }

        [Range(1, 5)]
        [Required]
        public int Rating { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public string Picture { get; set; }
    }
}
