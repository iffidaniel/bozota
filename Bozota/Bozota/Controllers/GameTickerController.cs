using Bozota.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bozota.Controllers
{
    [ApiController]
    [Route("game/ticker")]
    public class GameTickerController : ControllerBase
    {
        private readonly ILogger<GameTickerController> _logger;
        private readonly GameTickerService _gameTicker;

        public GameTickerController(ILogger<GameTickerController> logger,
            GameTickerService gameTicker)
        {
            _logger = logger;
            _gameTicker = gameTicker;
        }

        [HttpGet]
        [Route("start")]
        public ActionResult StartTicker()
        {
            _logger.LogTrace("{request} requested", nameof(StartTicker));

            _gameTicker.StartGameTicker();

            return Ok();
        }

        [HttpGet]
        [Route("stop")]
        public async Task<ActionResult> StopTicker()
        {
            _logger.LogTrace("{request} requested", nameof(StopTicker));

            await _gameTicker.StopGameTicker();

            return Ok();
        }

        [HttpGet]
        [Route("interval")]
        public ActionResult<int> GetTickerInterval()
        {
            _logger.LogTrace("{request} requested", nameof(GetTickerInterval));

            return _gameTicker.GetTickerInterval();
        }

        [HttpPost]
        [Route("interval/{interval}")]
        public async Task<ActionResult<int>> SetTickerInterval(int interval)
        {
            _logger.LogTrace("{request} requested", nameof(SetTickerInterval));

            return await _gameTicker.SetTickerInterval(interval);
        }
    }
}
