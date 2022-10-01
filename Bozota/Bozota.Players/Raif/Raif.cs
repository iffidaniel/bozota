using Bozota.Common.Models;
using Bozota.Common.Models.Items;
using Bozota.Common.Models.Items.Abstractions;
using Bozota.Common.Models.Players;
using Bozota.Players.Utils;

namespace Bozota.Players.Raif;

public class Raif : IPlayingPlayer
{
    public string Name => "Raif";
    private Player _me = new();

    private int _continuousShoots = 0;
    private Direction _shootDirection = Direction.None;
    private Position _previousPosition = new();
    private bool _previousActionWasMove = false;

    public PlayerAction NextAction(GameStateUtils gameStateUtils)
    {
        var state = gameStateUtils.GameState;
        _me = state.Players.First(p => p.Name == Name);

        PlayerAction? playerAction = null;
        if (_me.Ammo > 0)
        {
            // Shoot
            playerAction = ActionUtils.ShootClosest(Name, state.Players);
            if (_continuousShoots > 3 && _shootDirection == playerAction?.Direction)
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
            // Move
            ResetShoots();

            var myPosition = new Position { X = _me.XPos, Y = _me.YPos };
            var health = gameStateUtils.FindClosestHealthItem(myPosition);
            var ammo = gameStateUtils.FindClosestAmmoItem(myPosition);
            int distHealth = health is null ? 1000 : DataUtils.GetDistance(myPosition, new Position { X = health.XPos, Y = health.YPos });
            int distAmmo = ammo is null ? 1000 : DataUtils.GetDistance(myPosition, new Position { X = ammo.XPos, Y = ammo.YPos });

            if (_me.Health.Points <= 50 || distHealth <= distAmmo)
                playerAction = MoveTo(health);
            else
                playerAction = MoveTo(ammo);

            if (playerAction is null || _previousActionWasMove && _previousPosition.X == _me.XPos && _previousPosition.Y == _me.YPos)
                playerAction = MoveRandomly(gameStateUtils);

            _previousActionWasMove = true;
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

    private PlayerAction? MoveRandomly(GameStateUtils stateUtils)
    {
        int testDirection;
        Random r = new Random();    
        testDirection = r.Next(1, 4);
        return new PlayerAction(Name, GameAction.Move, (Direction)testDirection);
        Direction chosenDirection = Direction.None;

        for (int i = 0; i < 4; i++)
        {
            switch(testDirection)
            {
                case (int)Direction.Up:
                    if (stateUtils.TakenPositions.Any(p => p.X == _me.XPos && p.Y == _me.YPos - 1))
                    {
                        chosenDirection = Direction.Up;
                    }
                    break;
                case (int)Direction.Down:
                    if (stateUtils.TakenPositions.Any(p => p.X == _me.XPos && p.Y == _me.YPos + 1))
                    {
                        chosenDirection = Direction.Down;
                    }
                    break;
                case (int)Direction.Left:
                    if (stateUtils.TakenPositions.Any(p => p.X == _me.XPos - 1 && p.Y == _me.YPos))
                    {
                        chosenDirection = Direction.Left;
                    }
                    break;
                case (int)Direction.Right:
                    if (stateUtils.TakenPositions.Any(p => p.X == _me.XPos + 1 && p.Y == _me.YPos))
                    {
                        chosenDirection = Direction.Right;
                    }
                    break;
            }
            if (chosenDirection == Direction.None)
            {
                if (testDirection == 4)
                    testDirection = 1;
                else
                    testDirection++;
            }
            else
            {
                break;
            }
        }
        return new PlayerAction(Name, GameAction.Move, chosenDirection);
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

}
