namespace UrlShortener.Models
{
    public class UrlShortened
    {
        public string Id { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
