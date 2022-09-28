using Bozota.Common.Models;

namespace Bozota.Test.Models;

public class HealthTest
{
    public HealthTest()
    {
    }

    [Theory]
    [InlineData(100, 100, 0, 1)]
    [InlineData(10, 10, 0, 1)]
    [InlineData(1, 1, 0, 1)]
    [InlineData(0, 1, 0, 1)]
    [InlineData(-1, 1, 0, 1)]
    [InlineData(-10, 1, 0, 1)]
    public void NewHealthObjectWithoutOptionalParameters(int inPoints, int outPoints, int outMaxPoints, int outMinPoints)
    {
        Health health = new(inPoints);
        Assert.Equal(outPoints, health.Points);
        Assert.Equal(outMaxPoints, health.MaxPoints);
        Assert.Equal(outMinPoints, health.MinPoints);
        Assert.True(health.IsAlive);
        Assert.False(health.IsInDestructable);
    }

    [Theory]
    [InlineData(100, 100, 100, 100, 1)]
    [InlineData(100, 101, 100, 101, 1)]
    [InlineData(101, 100, 100, 100, 1)]
    [InlineData(100, 0, 100, 0, 1)]
    [InlineData(100, -1, 1, 1, 1)]
    [InlineData(100, -10, 1, 1, 1)]
    public void NewHealthObjectWithFirstOptionalParameter(int inPoints, int inMaxPoints, int outPoints, int outMaxPoints, int outMinPoints)
    {
        Health health = new(inPoints, inMaxPoints);
        Assert.Equal(outPoints, health.Points);
        Assert.Equal(outMaxPoints, health.MaxPoints);
        Assert.Equal(outMinPoints, health.MinPoints);
        Assert.True(health.IsAlive);
        Assert.False(health.IsInDestructable);
    }

    [Theory]
    [InlineData(100, 100, 1, 100, 100, 1)]
    [InlineData(100, 100, 10, 100, 100, 10)]
    [InlineData(100, 100, 100, 100, 100, 100)]
    [InlineData(100, 100, 0, 100, 100, 1)]
    [InlineData(100, 100, -1, 100, 100, 1)]
    [InlineData(100, 100, -10, 100, 100, 1)]
    [InlineData(100, 1, 100, 100, 100, 100)]
    [InlineData(100, 1, 10, 10, 10, 10)]
    [InlineData(-100, 1, 10, 10, 10, 10)]
    public void NewHealthObjectWithSecondOptionalParameter(int inPoints, int inMaxPoints, int inMinPoints, int outPoints, int outMaxPoints, int outMinPoints)
    {
        Health health = new(inPoints, inMaxPoints, inMinPoints);
        Assert.Equal(outPoints, health.Points);
        Assert.Equal(outMaxPoints, health.MaxPoints);
        Assert.Equal(outMinPoints, health.MinPoints);
        Assert.True(health.IsAlive);
        Assert.False(health.IsInDestructable);
    }

    [Theory]
    [InlineData(100, 100, 1, 50, 50, true)]
    [InlineData(10, 100, 1, 50, 0, false)]
    [InlineData(100, 1000, 1, 1000, 0, false)]
    [InlineData(2, 10, 1, 1, 1, true)]
    [InlineData(1, 1, 1, 1, 0, false)]
    [InlineData(100, 100, 1, -1, 100, true)]
    [InlineData(100, 1000, 100, -100, 100, true)]
    public void DealLethalDamage(int inPoint, int maxPoints, int minPoints, int damage, int outPoints, bool isAlive)
    {
        Health health = new(inPoint, maxPoints, minPoints);
        bool result = health.Damage(damage);

        Assert.Equal(outPoints, health.Points);
        Assert.Equal(isAlive, result);
        Assert.Equal(isAlive, health.IsAlive);
    }

    [Theory]
    [InlineData(100, 100, 1, 50, 50, true)]
    [InlineData(100, 100, 10, 50, 50, true)]
    [InlineData(10, 100, 1, 50, 1, true)]
    [InlineData(1, 100, 10, 100, 10, true)]
    [InlineData(100, 1000, 1, 1000, 1, true)]
    [InlineData(2, 10, 1, 1, 1, true)]
    [InlineData(1, 1, 1, 1, 1, true)]
    [InlineData(100, 100, 1, -1, 100, true)]
    [InlineData(100, 1000, 100, -100, 100, true)]
    public void DealNonLethalDamage(int inPoint, int maxPoints, int minPoints, int damage, int outPoints, bool isAlive)
    {
        Health health = new(inPoint, maxPoints, minPoints);
        bool result = health.Damage(damage, false);

        Assert.Equal(outPoints, health.Points);
        Assert.Equal(isAlive, result);
        Assert.True(health.IsAlive);
    }

    [Theory]
    [InlineData(100, 1000, 1, 50, 150)]
    [InlineData(100, 150, 1, 50, 150)]
    [InlineData(100, 200, 1, 500, 200)]
    [InlineData(10, 200, 1, 50, 60)]
    [InlineData(10, 200, 1, -50, 10)]
    public void RestoreHealth(int inPoint, int maxPoints, int minPoints, int restore, int outPoints)
    {
        Health health = new(inPoint, maxPoints, minPoints);
        health.Restore(restore);

        Assert.Equal(outPoints, health.Points);
        Assert.True(health.IsAlive);
    }

    [Fact]
    public void HealthObjectIsInDestructable()
    {
        Health health = new(100, default, 1, true);
        Assert.True(health.IsInDestructable);

        health.Damage(100, false);
        Assert.Equal(100, health.Points);

        health.Damage(100);
        Assert.Equal(100, health.Points);
    }
}
