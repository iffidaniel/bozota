using Bozota.Common.Models;
using Bozota.Players;
using Bozota.Players.Daniel;
using Bozota.Players.DummyPlayer;
using Bozota.Players.Veikko;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var playerNames = app.Configuration.GetSection("Game:Players").GetChildren().Select(x => x.Value)?.ToList() ?? new List<string>();
var players = new List<IPlayingPlayer>();
foreach (var playerName in playerNames)
{
    switch (playerName)
    {
        case "Daniel":
            players.Add(new BestPlayer());
            break;
        case "Veikko":
            players.Add(new Veikko());
            break;
        case "Krishna":
            players.Add(new DummyPlayer(playerName));
            break;
        case "Raif":
            players.Add(new DummyPlayer(playerName));
            break;
        case "Ramesh":
            players.Add(new DummyPlayer(playerName));
            break;
        case "Riku":
            players.Add(new DummyPlayer(playerName));
            break;
    }
}

var playerUtils = new PlayerUtils();

app.MapGet("/all/players", () => playerNames);

app.MapPost("/all/player/actions", ([FromBody] string state) =>
{
    var actions = new List<PlayerAction>();
    try
    {
        var gameState = JsonSerializer.Deserialize<GameState>(state);

        if (gameState is not null)
        {
            playerUtils.ProcessGameState(gameState);

            foreach (var player in players)
            {
                actions.Add(player.NextAction(gameState, playerUtils));
            }
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError("Failed to get next player actions", ex.Message);
    }

    return JsonSerializer.Serialize(actions);
});

app.Logger.LogInformation("The player API has started");

app.Run();
