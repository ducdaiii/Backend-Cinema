using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace CinemaHD.Models.Domains
{
    public class Users
    {
        [Key]
        public string UserID { get; set; }
        public string NameUse { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string MemberType { get; set; } = "Member";
        public int Score { get; set; } = 0;
        public string Avatar { get; set; } = "Add your avarta";
        public int? RoleID { get; set; }
        public bool IsVerified { get; set; } = true;

        [ForeignKey("RoleID")]
        public Roles Role { get; set; }
    }
}
