namespace CinemaHD.Models.DTOs
{
    public class SeatDTO
    {
        public string CinemaName { get; set; }
        public string SeatPosition { get; set; }
        public bool IsBooked { get; set; }
        public bool IsActive { get; set; }
    }
}
