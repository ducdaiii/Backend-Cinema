namespace CinemaHD.Models.DTOs
{
    public class ShiftDTO
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
    }
}
