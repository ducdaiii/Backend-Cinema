namespace CinemaHD.Models.DTOs
{
    public class ReservationDTO
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int SeatID { get; set; }
        public string CinemaName { get; set; }
        public string SeatPosition { get; set; }
        public DateTime TimeReservation { get; set; }
    }
}
