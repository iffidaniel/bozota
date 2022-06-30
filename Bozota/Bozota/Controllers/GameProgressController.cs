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
        public async Task<ActionResult<bool>> StartGame()
        {
            _logger.LogTrace("{request} requested", nameof(StartGame));

            await _gameProgress.InitializeGameAsync();

            return _gameProgress.IsGameInitialized();
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
