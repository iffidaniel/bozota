namespace Bozota.Models;

public class GameTicker
{
    private int interval = 500;
    private int minInterval = 10;
    private int maxInterval = 2000;

    public int Interval
    {
        get { return interval; }
        set
        {
            if (value <= minInterval)
            {
                interval = minInterval;
            }
            else if (value >= maxInterval)
            {
                interval = maxInterval;
            }
            else
            {
                interval = value;
            }
        }
    }

    public int MinInterval
    {
        get { return minInterval; }
        set
        {
            if (value <= 1)
            {
                minInterval = 1;
            }
            else if (value >= maxInterval)
            {
                minInterval = maxInterval;
            }
            else
            {
                minInterval = value;
            }

            Interval = interval;
        }
    }

    public int MaxInterval
    {
        get { return maxInterval; }
        set
        {
            if (value <= minInterval)
            {
                maxInterval = minInterval;
            }
            else if (value >= 3600000)
            {
                maxInterval = 3600000;
            }
            else
            {
                maxInterval = value;
            }

            Interval = interval;
        }
    }

    public bool IsRunning { get; set; } = false;

    public GameTicker(int minInterval, int maxInterval)
    {
        if (minInterval > this.maxInterval)
        {
            MaxInterval = maxInterval;
            MinInterval = minInterval;
        }
        else
        {
            MinInterval = minInterval;
            MaxInterval = maxInterval;
        }
    }
}
