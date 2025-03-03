using System.ComponentModel.DataAnnotations;

namespace CinemaHD.Models.Domains
{
    public class Locations
    {
        [Key]
        public int LocationID { get; set; }
        public string NameLocation { get; set; }
        public string AddressBe { get; set; }
    }
}
