namespace CinemaHD.Models.DTOs
{
    public class OTPDTO
    {
        public string OTPCode { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsUsed { get; set; }
        public string UserID { get; set; }
    }
}
