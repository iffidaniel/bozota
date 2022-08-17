using Bozota.Models;
using Bozota.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bozota.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameMasterController : ControllerBase
    {
        private readonly ILogger<GameMasterController> _logger;
        private readonly GameMasterService _gameProgress;

        public GameMasterController(ILogger<GameMasterController> logger,
            GameMasterService gameProgress)
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
        [Route("update")]
        public async Task<ActionResult<GameMap?>> UpdateGame()
        {
            _logger.LogTrace("{request} requested", nameof(UpdateGame));

            return await _gameProgress.UpdateGameAsync();
        }
    }
}
