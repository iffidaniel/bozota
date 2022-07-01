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
        [Route("init")]
        public async Task<ActionResult<GameMap?>> InitializeGame()
        {
            _logger.LogTrace("{request} requested", nameof(InitializeGame));

            return await _gameProgress.InitializeGameAsync();
        }

        [HttpGet]
        [Route("status")]
        public ActionResult<GameMap?> GetGameStatus()
        {
            _logger.LogTrace("{request} requested", nameof(GetGameStatus));

            return _gameProgress.GetGameProgress();
        }
    }
}
