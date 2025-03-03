namespace CinemaHD.Models.DTOs
{
    public class SeatDTO
    {
        public int CinemaID { get; set; }
        public string CinemaName { get; set; }
        public string SeatPosition { get; set; }
        public bool IsBooked { get; set; }
    }
}
