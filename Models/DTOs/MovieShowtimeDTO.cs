namespace CinemaHD.Models.DTOs
{
    public class MovieShowtimeDTO
    {
        public string MovieID { get; set; }
        public string MovieTitle { get; set; }
        public int CinemaID { get; set; }
        public string CinemaName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
