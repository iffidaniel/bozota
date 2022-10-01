using Bozota.Common.Models;
using Bozota.Common.Models.Items;
using Bozota.Common.Models.Items.Abstractions;
using Bozota.Common.Models.Objects.Abstractions;
using Bozota.Common.Models.Players;
using Bozota.Players.Utils;

namespace Bozota.Players.Raif;

public class Raif : IPlayingPlayer
{
    public string Name => "Raif";
    private int _bombExplosionRadius = 5;
    private Player _me = new();
    private GameState _gameState = new();

    private int _continuousShoots = 0;
    private Direction _shootDirection = Direction.None;
    private Position _previousPosition = new();
    private bool _previousActionWasMove = false;
    private bool _previousShotBomb = false;

    public PlayerAction NextAction(GameStateUtils gameStateUtils)
    {
        _gameState = gameStateUtils.GameState;
        _me = _gameState.Players.First(p => p.Name == Name);

        PlayerAction? playerAction = null;
        if (_me.Ammo > 0)
        {
            playerAction = ActionUtils.ShootClosest(Name, _gameState.Players);
            if (playerAction is null && !_previousShotBomb)
            {
                playerAction = ShootBomb();
                _previousShotBomb = true;
            }
            else
            {
                _previousShotBomb = false;
            }
            if (_continuousShoots > 2 && _shootDirection == playerAction?.Direction)
            {
                playerAction = null;
            }
            if (playerAction is not null)
            {
                _continuousShoots++;
                _shootDirection = playerAction.Direction;
            }
        }
        if (playerAction is null)
        {
            ResetShoots();

            var myPosition = new Position { X = _me.XPos, Y = _me.YPos };
            var health = gameStateUtils.FindClosestHealthItem(myPosition);
            var ammo = gameStateUtils.FindClosestAmmoItem(myPosition);
            int distHealth = health is null ? 1000 : DataUtils.GetDistance(myPosition, new Position { X = health.XPos, Y = health.YPos });
            int distAmmo = ammo is null ? 1000 : DataUtils.GetDistance(myPosition, new Position { X = ammo.XPos, Y = ammo.YPos });

            if (distHealth <= distAmmo)
                playerAction = MoveTo(health);
            else
                playerAction = MoveTo(ammo);

            if (_previousActionWasMove && _previousPosition.X == _me.XPos && _previousPosition.Y == _me.YPos)
            {
                // Shoot wall
                playerAction = new PlayerAction(Name, GameAction.Shoot, playerAction.Direction);
                _previousActionWasMove = false;
            }
            else
                _previousActionWasMove = true;

            if (playerAction is null)
            {
                playerAction = RandomMove(gameStateUtils);
            }
        }
        else
        {
            _previousActionWasMove = false;
        }

        _previousPosition.X = _me.XPos;
        _previousPosition.Y = _me.YPos;

        return playerAction;
    }

    private void ResetShoots()
    {
        _continuousShoots = 0;
        _shootDirection = Direction.None;
    }

    private PlayerAction? RandomMove(GameStateUtils stateUtils)
    {
        int testDirection;
        Random r = new Random();    
        testDirection = r.Next(1, 4);
        return new PlayerAction(Name, GameAction.Move, (Direction)testDirection);

        //Direction chosenDirection = Direction.None;

        //for (int i = 0; i < 4; i++)
        //{
        //    switch(testDirection)
        //    {
        //        case (int)Direction.Up:
        //            if (stateUtils.TakenPositions.Any(p => p.X == _me.XPos && p.Y == _me.YPos - 1))
        //            {
        //                chosenDirection = Direction.Up;
        //            }
        //            break;
        //        case (int)Direction.Down:
        //            if (stateUtils.TakenPositions.Any(p => p.X == _me.XPos && p.Y == _me.YPos + 1))
        //            {
        //                chosenDirection = Direction.Down;
        //            }
        //            break;
        //        case (int)Direction.Left:
        //            if (stateUtils.TakenPositions.Any(p => p.X == _me.XPos - 1 && p.Y == _me.YPos))
        //            {
        //                chosenDirection = Direction.Left;
        //            }
        //            break;
        //        case (int)Direction.Right:
        //            if (stateUtils.TakenPositions.Any(p => p.X == _me.XPos + 1 && p.Y == _me.YPos))
        //            {
        //                chosenDirection = Direction.Right;
        //            }
        //            break;
        //    }
        //    if (chosenDirection == Direction.None)
        //    {
        //        if (testDirection == 4)
        //            testDirection = 1;
        //        else
        //            testDirection++;
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}
        //return new PlayerAction(Name, GameAction.Move, chosenDirection);
    }

    private PlayerAction? MoveTo(IMapItem? mapItem)
    {
        if (mapItem is null)
            return null;

        if (mapItem.XPos < _me.XPos)
        {
            return new PlayerAction(Name, GameAction.Move, Direction.Left);
        }
        else if (mapItem.XPos > _me.XPos)
        {
            return new PlayerAction(Name, GameAction.Move, Direction.Right);
        }
        else
        {
            if (mapItem.YPos > _me.YPos)
            {
                return new PlayerAction(Name, GameAction.Move, Direction.Up);
            }
            else
            {
                return new PlayerAction(Name, GameAction.Move, Direction.Down);
            }
        }
    }

    private PlayerAction? ShootBomb()
    {
        PlayerAction? action = null;
        BombMetaData bombToShoot = null;
        var bombs = GetBombsInAxisSectors();
        List<Direction> invalidDirections = new List<Direction>();

        foreach (var b in bombs)
        {
            if (b.Distance <= _bombExplosionRadius)
            {
                if (!invalidDirections.Contains(b.Direction))
                    invalidDirections.Add(b.Direction);
            }
        }

        foreach (var b in bombs)
        {
            if (!invalidDirections.Contains(b.Direction) && b.Distance > _bombExplosionRadius && (bombToShoot is null || bombToShoot.Distance > b.Distance))
                bombToShoot = b;
        }

        if (bombToShoot is not null)
            action = new PlayerAction(Name, GameAction.Shoot, bombToShoot.Direction);
        
        return action;
    }

    private List<BombMetaData> GetBombsInAxisSectors()
    {
        List<BombMetaData> bombs = new List<BombMetaData>();
        foreach (var b in _gameState.Bombs)
        {
            var distance = DataUtils.GetDistance(new Position { X = _me.XPos, Y = _me.YPos }, new Position { X = b.XPos, Y = b.YPos });
            if (b.XPos == _me.XPos)
            {
                bombs.Add(
                new BombMetaData(
                    b.YPos > _me.YPos ? Direction.Up : Direction.Down,
                    distance,
                    b));
            }
            else if (b.YPos == _me.YPos)
            {
                bombs.Add(
                new BombMetaData(
                    b.XPos > _me.XPos ? Direction.Right : Direction.Left,
                    distance,
                    b));
            }
        }
        return bombs;
    }

    //private IBombObject? FindClosestBomb(Position origin, GameState gameState)
    //{
    //    if (gameState.Bombs.Count <= 0)
    //    {
    //        return null;
    //    }

    //    IBombObject? itemWithLeastDistance = gameState.Bombs[0];
    //    var leastDistance = DataUtils.GetDistance(origin, new Position { X = gameState.Bombs[0].XPos, Y = gameState.Bombs[0].YPos });
    //    foreach (var item in gameState.Bombs)
    //    {
    //        var distance = DataUtils.GetDistance(origin, new Position { X = item.XPos, Y = item.YPos });
    //        if (distance < leastDistance)
    //        {
    //            leastDistance = distance;
    //            itemWithLeastDistance = item;
    //        }
    //    }

    //    return itemWithLeastDistance;
    //}

    private class BombMetaData
    {
        public Direction Direction { get; set; }

        public int Distance { get; set; }

        public IBombObject BombObject { get; set; }

        public BombMetaData(Direction direction, int distance, IBombObject bombObject)
        {
            Direction = direction;
            Distance = distance;
            BombObject = bombObject;
        }
    }

}
