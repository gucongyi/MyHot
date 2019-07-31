using System; 
public class Mathd
{ 
    public const double Deg2Rad = Math.PI * 2d / 360d;
    public const double Rad2Deg = 1d / Deg2Rad;
    public static double Lerp(double a, double b, double t)
    {
        return a + (b - a) * Clamp01(t);
    }

    public static double Clamp01(double value)
    {
        if (value < 0d)
            return 0d;
        else if (value > 1d)
            return 1d;
        else
            return value;
    }

    public static double Clamp(double value, double min, double max)
    {
        if (value < min)
            value = min;
        else if (value > max)
            value = max;
        return value;
    }

    public static double LerpAngle(double a, double b, double t)
    {
        double delta = Repeat((b - a), 360);
        if (delta > 180)
            delta -= 360;
        return a + delta * Clamp01(t);
    }

    public static double Repeat(double t, double length)
    {
        return Clamp(t - Math.Floor(t / length) * length, 0.0f, length);
    }
} 
