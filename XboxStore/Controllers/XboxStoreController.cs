using Microsoft.AspNetCore.Mvc;
using XboxStore.Services;

namespace XboxStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class XboxStoreController : ControllerBase
    {
        private readonly IXboxGameService _xboxService;

        public XboxStoreController(IXboxGameService xboxService)
        {
            this._xboxService = xboxService;
        }

        [HttpGet]
        public async Task<IActionResult> GetXboxGames()
        {
            var xboxGames = await this._xboxService.GetXboxGames();

            return Ok(xboxGames);
        }
    }
}
