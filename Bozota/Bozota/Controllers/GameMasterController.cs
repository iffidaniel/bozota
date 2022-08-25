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
        private readonly GameMasterService _gameMaster;

        private bool _isGameInitializing = false;
        private bool _isGameUpdating = false;

        public GameMasterController(ILogger<GameMasterController> logger,
            GameMasterService gameMaster)
        {
            _logger = logger;
            _gameMaster = gameMaster;
        }

        [HttpGet]
        [Route("init")]
        public async Task<ActionResult<GameState?>> InitializeGame()
        {
            _logger.LogTrace("{request} requested", nameof(InitializeGame));

            GameState? gameMap = null;

            try
            {
                if (!_isGameInitializing)
                {
                    _isGameInitializing = true;
                    gameMap = await _gameMaster.InitializeGameAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update game {exception}", ex);
            }
            finally
            {
                _isGameInitializing = false;
            }

            return gameMap;
        }

        [HttpGet]
        [Route("update")]
        public async Task<ActionResult<GameState?>> UpdateGame()
        {
            _logger.LogTrace("{request} requested", nameof(UpdateGame));

            GameState? gameMap = null;

            try
            {
                if (!_isGameUpdating)
                {
                    _isGameUpdating = true;
                    gameMap = await _gameMaster.UpdateGameAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update game {exception}", ex);
            }
            finally
            {
                _isGameUpdating = false;
            }

            return gameMap;
        }
    }
}
