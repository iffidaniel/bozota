namespace Bozota.Models
{
    public class Health
    {
        public int Points { get; private set; }

        public int MaxPoints { get; }

        public int MinPoints { get; }

        /// <summary>
        /// Determines whether health can take any damage
        /// </summary>
        public bool IsInDestructable;

        /// <summary>
        /// Shows whether health is higher than zero
        /// </summary>
        public bool IsAlive => Points > 0;

        /// <summary>
        /// Instantiates a new health object.
        /// </summary>
        /// <param name="startingPoints">Starting health points</param>
        /// <param name="maxPoints">Maximum amount of health points that can be acquired</param>
        /// <param name="minPoints">Minimum amount of health points for non-lethal damage</param>
        public Health(int startingPoints, int maxPoints = default, int minPoints = 1, bool isInDestructable = false)
        {
            Points = startingPoints;
            MaxPoints = maxPoints;
            MinPoints = minPoints;
            IsInDestructable = isInDestructable;

            if (MinPoints < 1)
            {
                MinPoints = 1;
            }

            if (MaxPoints < MinPoints && MaxPoints != default)
            {
                MaxPoints = MinPoints;
            }

            if (Points > MaxPoints && MaxPoints != default)
            {
                Points = MaxPoints;
            }

            if (Points < MinPoints)
            {
                Points = MinPoints;
            }
        }

        /// <summary>
        /// Deducts damage from health points
        /// </summary>
        /// <param name="damage">Amount of damage being dealt</param>
        /// <param name="lethal">Determines whether damage is lethal or non-lethal.
        /// Non-lethal damage cannot drop below the minimum amount of healthpoints.
        /// Default value is true, meaning damage is lethal</param>
        /// <returns>If health drops to zero, return false, else true</returns>
        public bool Damage(int damage, bool lethal = true)
        {
            if (IsInDestructable)
            {
                return true;
            }

            if (damage < 0)
            {
                damage = 0;
            }

            Points -= damage;

            if (!lethal && Points < MinPoints)
            {
                Points = MinPoints;
            }

            if (Points < 0)
            {
                Points = 0;
            }

            if (Points == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Restore health points up until maximum amount of health points if health points exceed this
        /// </summary>
        /// <param name="points">Amount of points being restored</param>
        public void Restore(int points)
        {
            if (points < 0)
            {
                points = 0;
            }

            Points += points;

            if (Points > MaxPoints)
            {
                Points = MaxPoints;
            }
        }
    }
}
