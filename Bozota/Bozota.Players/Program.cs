using Bozota.Common.Models;
using Bozota.Players;
using Bozota.Players.DummyPlayer;
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
            players.Add(new DummyPlayer(playerName));
            break;
        case "Veikko":
            players.Add(new DummyPlayer(playerName));
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

app.MapGet("/all/players", () => playerNames);

app.MapPost("/all/player/actions", ([FromBody]string gameState) =>
{
    var actions = new List<PlayerAction>();
    try
    {
        var state = JsonSerializer.Deserialize<GameState>(gameState);

        if (state is not null)
        {
            foreach (var player in players)
            {
                actions.Add(player.NextAction(state));
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
