using Bozota.Controllers;
using Bozota.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics;

namespace Bozota.Test
{
    public class GameTickerTest
    {
        private readonly GameProgressController gameProgressController;
        private readonly GameTickerController gameTickerController;

        public GameTickerTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var gameLogicServiceLogger = new Mock<ILogger<GameLogicService>>(MockBehavior.Loose);
            var gameLogicService = new GameLogicService(gameLogicServiceLogger.Object, config);

            var gameProgressServiceLogger = new Mock<ILogger<GameProgressService>>(MockBehavior.Loose);
            var gameProgressService = new GameProgressService(gameProgressServiceLogger.Object, config, gameLogicService);

            var gameTickerServiceLogger = new Mock<ILogger<GameTickerService>>(MockBehavior.Loose);
            var gameTickerService = new GameTickerService(gameTickerServiceLogger.Object, config, gameProgressService);

            var gameProgressControllerLogger = new Mock<ILogger<GameProgressController>>(MockBehavior.Loose);
            gameProgressController = new GameProgressController(gameProgressControllerLogger.Object, gameProgressService);

            var gameTickerControllerLogger = new Mock<ILogger<GameTickerController>>(MockBehavior.Loose);
            gameTickerController = new GameTickerController(gameTickerControllerLogger.Object, gameTickerService);
        }

        [Theory]
        [InlineData(1000, 10)]
        [InlineData(1000000, 10000)]
        public async Task GameProgressCanBeRequestedXTimesWithinTimeSpan(int amountOfRequests, int duration)
        {
            Stopwatch stopWatch = new();
            await gameProgressController.InitializeGame();
            await gameTickerController.StartTicker();

            stopWatch.Start();
            for (int i=0; i<amountOfRequests; i++)
            {
                await gameProgressController.GetGameStatus();
            }
            stopWatch.Stop();

            Assert.True(stopWatch.Elapsed.Milliseconds < duration);
        }
    }
}