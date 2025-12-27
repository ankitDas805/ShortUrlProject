using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
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
       var shortUrl =  await shortUrlService.Insert(longUrl,Guid.NewGuid().ToString()[..4]);
        return Ok(shortUrl);
    }

    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> GetLongUrl(string shortUrl)
    {
        var response =  await shortUrlService.GetLongUrl(shortUrl);
        return Redirect(response);
    }
    
}
