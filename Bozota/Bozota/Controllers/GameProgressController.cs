using Bozota.Models;
using Bozota.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bozota.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameProgressController : ControllerBase
    {
        private readonly ILogger<GameProgressController> _logger;
        private readonly GameProgressService _gameProgress;

        public GameProgressController(ILogger<GameProgressController> logger,
            GameProgressService gameProgress)
        {
            _logger = logger;
            _gameProgress = gameProgress;
        }

        [HttpGet]
        [Route("start")]
        public async Task<ActionResult> StartGame()
        {
            _logger.LogTrace("{request} requested", nameof(StartGame));

            await _gameProgress.InitializeGameAsync();

            if (!_gameProgress.IsGameInitialized())
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpGet]
        [Route("progress")]
        public async Task<ActionResult<GameMap?>> GetGameProgress()
        {
            _logger.LogTrace("{request} requested", nameof(GetGameProgress));

            return await _gameProgress.GetGameProgress();
        }
    }
}
