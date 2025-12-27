using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ShortController : ControllerBase
{
    private readonly ShortUrlService shortUrlService;

    public ShortController(ShortUrlService shortUrlService)
    {
        this.shortUrlService = shortUrlService;
    }
    [HttpPost("Create")]
    public async Task<IActionResult> Create(string longUrl)
    {
        await shortUrlService.Insert(longUrl,Guid.NewGuid().ToString()[..4]);
        return Ok();
    }

    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> GetLongUrl(string shortUrl)
    {
        var response =  await shortUrlService.GetLongUrl(shortUrl);
        return Redirect(response);
    }

    
}
