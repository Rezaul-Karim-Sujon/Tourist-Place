using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Tourist_Place.Models.View_Model
{
    public class VMPlaces
    {
        //public int PlaceID { get; set; }
        public string PlaceName { get; set; }
        public string Address { get; set; }
        public int Rating { get; set; }
        public int Type { get; set; }
        public IFormFile Picture { get; set; }
    }
}
