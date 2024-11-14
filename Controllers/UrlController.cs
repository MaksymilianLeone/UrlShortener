using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Services.UrlService;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : Controller
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShortUrl([FromBody] CreateShortUrlDto dto)
        {
            TimeSpan? ttlValue = dto.Ttl.HasValue ? TimeSpan.FromSeconds(dto.Ttl.Value) : (TimeSpan?)null;

            var result = await _urlService.CreateShortUrl(dto.OriginalUrl, dto.CustomId, ttlValue);
            return CreatedAtAction(nameof(GetOriginalUrl), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOriginalUrl(string id)
        {
            try
            {
                var url = await _urlService.GetOriginalUrl(id);
                return Redirect(url);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShortUrl(string id)
        {
            await _urlService.DeleteShortUrl(id);
            return NoContent();
        }
    }
}
