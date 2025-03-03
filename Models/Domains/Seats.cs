using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaHD.Models.Domains
{
    public class Seats
    {
        [Key]
        public int SeatID { get; set; }
        public int CinemaID { get; set; }
        public string AtRow { get; set; }
        public int AtColumn { get; set; }
        public bool IsBooked { get; set; } = false;

        [ForeignKey("CinemaID")]
        public Cinemas Cinema { get; set; }
    }
}
