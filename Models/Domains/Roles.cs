using System.ComponentModel.DataAnnotations;

namespace CinemaHD.Models.Domains
{
    public class Roles
    {
        [Key]
        public int RoleID { get; set; }
        public string NameRole { get; set; }
    }
}
