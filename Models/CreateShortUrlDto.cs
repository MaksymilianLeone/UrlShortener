using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class CreateShortUrlDto
    {
        [Required]
        [Url]
        public string OriginalUrl { get; set; }

        public string CustomId { get; set; }

        public int? Ttl { get; set; }
    }
}
