using System.ComponentModel.DataAnnotations;

namespace CinemaHD.Models.Domains
{
    public class Locations
    {
        [Key]
        public int LocationID { get; set; }
        public string NameLocation { get; set; } = string.Empty;
        public string AddressBe { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
    }
}
