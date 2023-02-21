using Microsoft.AspNetCore.Mvc;
using TwitterScrapper.Services.Scrapper;

namespace TwitterScrapper.Controllers
{
    [ApiController]
    [Route("posts")]
    public class ScrapperController : ControllerBase
    {
        private readonly IScrapper _scrapper;

        public ScrapperController(IScrapper scrapper)
        {
            _scrapper = scrapper;
        }

        [HttpGet("get")]
        public IActionResult GetPosts()
        {
            var twitterCodeBlocks = _scrapper.Scrape();

            return Ok(twitterCodeBlocks);
        }
    }
}