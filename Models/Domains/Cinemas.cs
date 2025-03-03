using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaHD.Models.Domains
{
    public class Cinemas
    {
        [Key]
        public int CinemaID { get; set; }
        public string NameCinema { get; set; }
        public int LocationID { get; set; }

        [ForeignKey("LocationID")]
        public Locations Location { get; set; }
    }
}
