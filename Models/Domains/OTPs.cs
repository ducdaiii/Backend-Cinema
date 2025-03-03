using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaHD.Models.Domains
{
    public class OTPs
    {
        [Key]
        public int Id { get; set; }
        public string OTPCode { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsUsed { get; set; } = false;
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public Users User { get; set; }
    }
}
