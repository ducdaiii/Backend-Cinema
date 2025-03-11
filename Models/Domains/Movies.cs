using System.ComponentModel.DataAnnotations;

namespace CinemaHD.Models.Domains
{
    public class Movies
    {
        [Key]
        public string MovieID { get; set; } 
        public string Title { get; set; }
        public string Director { get; set; }
        public string Poster { get; set; }
        public string Details { get; set; }
        public string Genre { get; set; }
        public byte Duration { get; set; }
        public float Score { get; set; }
        public string Actors { get; set; }
        public string Country { get; set; }
        public string Subtitles { get; set; }
        public string Recommendations { get; set; }
        public DateTime ReleaseYear { get; set; }
        public string Trailer { get; set; }
    }
}
