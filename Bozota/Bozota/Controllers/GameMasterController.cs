using Bozota.Common.Models;
using Bozota.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bozota.Controllers;

[ApiController]
[Route("game")]
public class GameMasterController : ControllerBase
{
    private readonly ILogger<GameMasterController> _logger;
    private readonly GameMasterService _gameMaster;

    private bool _isGameResetting = false;
    private bool _isGameUpdating = false;

    public GameMasterController(ILogger<GameMasterController> logger,
        GameMasterService gameMaster)
    {
        _logger = logger;
        _gameMaster = gameMaster;
    }

    [HttpGet]
    [Route("reset")]
    public async Task ResetGame()
    {
        _logger.LogTrace("{request} requested", nameof(ResetGame));

        if (_isGameResetting)
        {
            _logger.LogInformation("Game already resetting");
            return;
        }

        try
        {
            _isGameResetting = true;
            await _gameMaster.StopGameAsync();
            await _gameMaster.InitializeGameAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to reset game {exception}", ex);
        }
        finally
        {
            _isGameResetting = false;
        }
    }

    [HttpGet]
    [Route("update")]
    public async Task<ActionResult<GameState?>> UpdateGame()
    {
        _logger.LogTrace("{request} requested", nameof(UpdateGame));

        GameState? gameMap = null;

        if (_isGameUpdating)
        {
            _logger.LogInformation("Game already updating");
            return gameMap;
        }

        try
        {
            _isGameUpdating = true;
            gameMap = await _gameMaster.UpdateGameAsync();
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
