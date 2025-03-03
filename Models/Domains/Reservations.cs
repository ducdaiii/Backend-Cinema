using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaHD.Models.Domains
{
    public class Reservations
    {
        [Key]
        public int ReservationID { get; set; }
        public string UserID { get; set; }
        public int SeatID { get; set; }
        public DateTime TimeReservation { get; set; }

        [ForeignKey("UserID")]
        public Users User { get; set; }

        [ForeignKey("SeatID")]
        public Seats Seat { get; set; }
    }
}
