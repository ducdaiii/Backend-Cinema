namespace CinemaHD.Models.DTOs
{
    public class MovieShowtimeDTO
    {
        public string MovieTitle { get; set; }
        public string CinemaName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
