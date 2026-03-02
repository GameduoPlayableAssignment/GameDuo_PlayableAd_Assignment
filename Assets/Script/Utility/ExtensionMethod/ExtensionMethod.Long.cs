using System;

public static partial class ExtensionMethod
{
    public static DateTime ToDateTime(this long value)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
    }

    public static long AddDays(this long value, int days)
    {
        return value.ToDateTime().AddDays(days).ToUnixTimestamp();
    }
    
    public static long AddMonths(this long value, int months)
    {
        return value.ToDateTime().AddMonths(months).ToUnixTimestamp();
    }
    
    public static long AddHours(this long value, int hours)
    {
        return value.ToDateTime().AddHours(hours).ToUnixTimestamp();
    }
    
    public static long AddMinutes(this long value, int minutes)
    {
        return value.ToDateTime().AddMinutes(minutes).ToUnixTimestamp();
    }
    
    public static long AddSeconds(this long value, int seconds)
    {
        return value.ToDateTime().AddSeconds(seconds).ToUnixTimestamp();
    }
}