using Bozota.Common.Models;
using Bozota.Common.Models.Items;
using Bozota.Common.Models.Items.Abstractions;
using Bozota.Common.Models.Objects.Abstractions;

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

        List<AmmoItem> consumedItems = new();
        foreach (Common.Models.Players.Player player in gameState.Players)
        {
            foreach (AmmoItem ammoItem in gameState.AmmoItems)
            {
                if (player.XPos == ammoItem.XPos && player.YPos == ammoItem.YPos)
                {
                    player.Ammo += ammoItem.Amount;
                    consumedItems.Add(ammoItem);
                }
            }

            // Remove consumed ammo items
            foreach (AmmoItem consumable in consumedItems)
            {
                gameState.AmmoItems.Remove(consumable);
            }
            consumedItems.Clear();
        }

        return Task.CompletedTask;
    }

    public Task ProcessMaterialsItems(GameState gameState)
    {
        _logger.LogDebug("Processing materials items");

        List<MaterialsItem> consumedItems = new();
        foreach (Common.Models.Players.Player player in gameState.Players)
        {
            foreach (MaterialsItem materialsItem in gameState.MaterialsItems)
            {
                if (player.XPos == materialsItem.XPos && player.YPos == materialsItem.YPos)
                {
                    player.Materials += materialsItem.Amount;
                    consumedItems.Add(materialsItem);
                }
            }

            // Remove consumed ammo items
            foreach (MaterialsItem consumable in consumedItems)
            {
                gameState.MaterialsItems.Remove(consumable);
            }
            consumedItems.Clear();
        }

        return Task.CompletedTask;
    }

    public Task ProcessHealthItems(GameState gameState)
    {
        _logger.LogDebug("Processing health items");

        List<HealthItem> consumedItems = new();
        foreach (Common.Models.Players.Player player in gameState.Players)
        {
            foreach (HealthItem healthItem in gameState.HealthItems)
            {
                if (player.XPos == healthItem.XPos && player.YPos == healthItem.YPos)
                {
                    player.Health.Restore(healthItem.HealAmount);
                    consumedItems.Add(healthItem);
                }
            }

            // Remove consumed health items
            foreach (HealthItem consumable in consumedItems)
            {
                gameState.HealthItems.Remove(consumable);
            }
            consumedItems.Clear();
        }

        return Task.CompletedTask;
    }

    public Task ProcessFires(GameState gameState)
    {
        _logger.LogDebug("Processing fire items");

        List<FireItem> dimmedFires = new();
        foreach (FireItem fire in gameState.FireItems)
        {
            if (fire.Duration <= 0)
            {
                dimmedFires.Add(fire);
                continue;
            }

            IsItemHitting(fire, gameState.Bombs);
            IsItemHitting(fire, gameState.Walls);
            IsItemHitting(fire, gameState.Players);

            fire.Duration -= 1;
        }

        // Remove dimmed fires
        foreach (FireItem fire in dimmedFires)
        {
            gameState.FireItems.Remove(fire);
        }

        return Task.CompletedTask;
    }

    public Task ProcessBullets(GameState gameState)
    {
        _logger.LogDebug("Processing bullets");

        List<BulletItem> bulletHits = new();
        int moveCounter = 0;
        bool movingBulletsLeft;
        do
        {
            movingBulletsLeft = false;

            foreach (BulletItem bullet in gameState.Bullets)
            {
                if (IsItemHitting(bullet, gameState.Players))
                {
                    bulletHits.Add(bullet);
                    continue;
                }

                if (IsItemHitting(bullet, gameState.Walls))
                {
                    bulletHits.Add(bullet);
                    continue;
                }

                if (IsItemHitting(bullet, gameState.Bombs))
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
            foreach (BulletItem bulletHit in bulletHits)
            {
                gameState.Bullets.Remove(bulletHit);
            }
            bulletHits.Clear();

            moveCounter++;
        }
        while (movingBulletsLeft && (moveCounter < gameState.MapXCellCount || moveCounter < gameState.MapYCellCount));

        return Task.CompletedTask;
    }

    public bool IsItemHitting<ITEM, T>(ITEM hittingItem, List<T> items) where ITEM : IDamageItem where T : IMapObject
    {
        foreach (T item in items)
        {
            if (item.XPos == hittingItem.XPos && item.YPos == hittingItem.YPos)
            {
                item.Health.Damage(hittingItem.DamageAmount);
                return true;
            }
        }

        return false;
    }
}
