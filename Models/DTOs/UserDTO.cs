namespace CinemaHD.Models.DTOs
{
    public class UserDTO
    {
        public string NameUse { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string MemberType { get; set; }
        public int Score { get; set; }
        public bool IsVerified { get; set; }
        public string RoleName { get; set; }
    }
}
