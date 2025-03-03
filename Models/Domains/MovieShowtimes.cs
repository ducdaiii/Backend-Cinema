using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaHD.Models.Domains
{
    public class MovieShowtimes
    {
        [Key]
        public int ShowtimeID { get; set; }
        public string MovieID { get; set; }
        public int CinemaID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [ForeignKey("MovieID")]
        public Movies Movie { get; set; }

        [ForeignKey("CinemaID")]
        public Cinemas Cinema { get; set; }
    }
}
