using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaHD.Models.Domains
{
    public class Shifts
    {
        [Key]
        public int ShiftID { get; set; }
        public string UserID { get; set; }
        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
        public int LocationID { get; set; }

        [ForeignKey("UserID")]
        public Users User { get; set; }

        [ForeignKey("LocationID")]
        public Locations Location { get; set; }
    }
}
