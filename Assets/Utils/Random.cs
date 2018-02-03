using System;
using System.Linq;

public static class Random
{
    private static readonly System.Random Rand;

    static Random()
    {
        Rand = new System.Random(Guid.NewGuid().GetHashCode());
    }

    public static T[] Shuffle<T>(T[] i)
    {
        return i.OrderBy(x => Rand.Next()).ToArray();
    }

    public static int Next(int min, int max)
    {
        return Rand.Next(min, max);
    }

    public static double Next(double min, double max)
    {
        return Rand.NextDouble() * (max - min) + min;
    }

    public static double NextGaussian(double mean, double standardDeviation)
    {
        double u1 = 1 - Rand.NextDouble();
        double u2 = 1 - Rand.NextDouble();
        double randStdNormal = Math.Sqrt(Math.Abs(-2f * Math.Log(u1) * Math.Sin(2 * Math.PI * u2)));
        double x = mean + standardDeviation * randStdNormal;

        return x;
    }
}