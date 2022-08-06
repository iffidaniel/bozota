using Bozota.Models;
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
        public async Task<ActionResult<GameTicker>> StartTicker()
        {
            _logger.LogTrace("{request} requested", nameof(StartTicker));

            await _gameTicker.StartGameTicker();

            return await _gameTicker.GetTicker();
        }

        [HttpGet]
        [Route("status")]
        public async Task<ActionResult<GameTicker>> GetTickerStatus()
        {
            _logger.LogTrace("{request} requested", nameof(GetTickerStatus));

            return await _gameTicker.GetTicker();
        }

        [HttpGet]
        [Route("stop")]
        public async Task<ActionResult<GameTicker>> StopTicker()
        {
            _logger.LogTrace("{request} requested", nameof(StopTicker));

            await _gameTicker.StopGameTicker();

            return await _gameTicker.GetTicker();
        }

        [HttpGet]
        [Route("interval/{interval}")]
        public async Task<ActionResult<GameTicker>> SetTickerInterval(int interval)
        {
            _logger.LogTrace("{request} requested", nameof(SetTickerInterval));

            return await _gameTicker.SetTickerInterval(interval);
        }
    }
}
