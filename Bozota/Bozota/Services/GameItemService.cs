using Bozota.Models;
using Bozota.Models.Map.Items.Abstractions;
using Bozota.Models.Map.Objects.Abstractions;

namespace Bozota.Services;

public class GameItemService
{
    private readonly ILogger<GameItemService> _logger;

    public GameItemService(ILogger<GameItemService> logger, IConfiguration config)
    {
        _logger = logger;
    }

    public Task ProcessAmmoItems(GameState gameState)
    {
        _logger.LogDebug("Processing ammo items");

        List<IAmmoItem> consumedItems = new();
        foreach (var player in gameState.Players)
        {
            foreach (var ammoItem in gameState.AmmoItems)
            {
                if (player.XPos == ammoItem.XPos && player.YPos == ammoItem.YPos)
                {
                    player.Ammo += ammoItem.Amount;
                    consumedItems.Add(ammoItem);
                }
            }

            // Remove consumed ammo items
            foreach (var consumable in consumedItems)
            {
                gameState.AmmoItems.Remove(consumable);
            }
            consumedItems.Clear();
        }

        return Task.CompletedTask;
    }

    public Task ProcessHealthItems(GameState gameState)
    {
        _logger.LogDebug("Processing health items");

        List<IHealthItem> consumedItems = new();
        foreach (var player in gameState.Players)
        {
            foreach (var healthItem in gameState.HealthItems)
            {
                if (player.XPos == healthItem.XPos && player.YPos == healthItem.YPos)
                {
                    player.Health.Restore(healthItem.HealAmount);
                    consumedItems.Add(healthItem);
                }
            }

            // Remove consumed health items
            foreach (var consumable in consumedItems)
            {
                gameState.HealthItems.Remove(consumable);
            }
            consumedItems.Clear();
        }

        return Task.CompletedTask;
    }

    public Task ProcessBullets(GameState gameState)
    {
        _logger.LogDebug("Processing bullets");

        List<IBulletItem> bulletHits = new();
        var moveCounter = 0;
        bool movingBulletsLeft;
        do
        {
            movingBulletsLeft = false;

            foreach (var bullet in gameState.Bullets)
            {
                if(IsBulletHitting(bullet, gameState.Players))
                {
                    bulletHits.Add(bullet);
                    continue;
                }

                if (IsBulletHitting(bullet, gameState.Walls))
                {
                    bulletHits.Add(bullet);
                    continue;
                }

                if (IsBulletHitting(bullet, gameState.Bombs))
                {
                    bulletHits.Add(bullet);
                    continue;
                }

                // If bullet does not hit an object, move forward
                if (bullet.Speed - moveCounter > 0)
                {
                    switch (bullet.Direction)
                    {
                        case Direction.Up:
                            if (bullet.YPos < gameState.MapYCellCount - 1)
                            {
                                bullet.YPos += 1;
                            }
                            else
                            {
                                bulletHits.Add(bullet);
                            }
                            break;
                        case Direction.Right:
                            if (bullet.XPos < gameState.MapXCellCount - 1)
                            {
                                bullet.XPos += 1;
                            }
                            else
                            {
                                bulletHits.Add(bullet);
                            }
                            break;
                        case Direction.Down:
                            if (bullet.YPos > 1)
                            {
                                bullet.YPos -= 1;
                            }
                            else
                            {
                                bulletHits.Add(bullet);
                            }
                            break;
                        case Direction.Left:
                            if (bullet.XPos > 1)
                            {
                                bullet.XPos -= 1;
                            }
                            else
                            {
                                bulletHits.Add(bullet);
                            }
                            break;
                        default:
                            bulletHits.Add(bullet);
                            break;
                    }

                    movingBulletsLeft = true;
                }
            }

            // Remove hit bullets
            foreach (var bulletHit in bulletHits)
            {
                gameState.Bullets.Remove(bulletHit);
            }
            bulletHits.Clear();

            moveCounter++;
        }
        while (movingBulletsLeft && (moveCounter < gameState.MapXCellCount || moveCounter < gameState.MapYCellCount));

        return Task.CompletedTask;
    }

    public bool IsBulletHitting<T>(IBulletItem bullet, List<T> items) where T : IMapObject
    {
        foreach (var item in items)
        {
            if (item.XPos == bullet.XPos && item.YPos == bullet.YPos)
            {
                item.Health.Damage(bullet.DamageAmount);
                return true;
            }
        }

        return false;
    }
}
